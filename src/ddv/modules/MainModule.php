<?php
namespace ddv\modules;

use ArithmeticError;
use std, gui, framework, ddv;
use php\io\Stream;
use php\util\Scanner;
use script\JsoupScript;
use php\io\IOException;
use php\lib\fs;
use php\lib\arr;
use php\util\Regex;

class MainModule extends AbstractModule {
	/**
	* @event chooserFileSave.action
	*/
	function doChooserFileSaveAction(ScriptEvent $e = null) {
		$this->clear->call();

		$GLOBALS['build.file'] = $this->chooserFileSave->file;

		Stream::putContents($GLOBALS['build.file'], "");

		$this->openBuild->call();
	}
	
	/**
	* @event chooserFileOpen.action
	*/
	function doChooserFileOpenAction(ScriptEvent $e = null) {
		$this->clear->call();

		$GLOBALS['build.file'] = $this->chooserFileOpen->file;

		$this->openBuild->call();
	}

	/**
	* @event openBuild.action
	*/
	function doOpenBuildAction(ScriptEvent $e = null) {
		try {
			$file = Stream::of($GLOBALS['build.file']);
			$scanner = new Scanner($file);

			while ($scanner->hasNextLine()) {
				$GLOBALS['list.count']++;

				$id = $scanner->nextLine();
				$GLOBALS['list.mod'][$id] = [];
			}

			$file->close();

			$this->form("MainForm")->title = "Сборка - " . fs::nameNoExt($GLOBALS['build.file']);
			$GLOBALS['build.opened'] = true;

			$this->loadContent->call();

		} catch (IOException $e) {
			$this->clear->call();
			UXDialog::showAndWait("Ошибка чтения файла!", 'ERROR');
		}
	}

	/**
	* @event clear.action
	*/
	function doClearAction(ScriptEvent $e = null) {
		$GLOBALS['list.mod'] = [];
		$GLOBALS['list.count'] = 0;

		$GLOBALS['getInfo.parse'] = 0;
		$GLOBALS['getInfo.error'] = 0;

		unset($GLOBALS['versions.available']);
		$GLOBALS['versions.codes'] = [];

		$GLOBALS['mod.downloaded'] = 0;

		$GLOBALS['build.opened'] = false;
		$GLOBALS['build.file'] = null;

		$this->modList->items->clear();

		$this->form("MainForm")->title = "Сборка не открыта";
	}

	/**
	* @event loadContent.action
	*/
	function doLoadContentAction(ScriptEvent $e = null) {
		if ($GLOBALS['list.count'] == 0) {
			return;
		}

		$this->form("MainForm")->showPreloader("Загрузка данных");

		foreach ($GLOBALS['list.mod'] as $index => $element) {
			$this->getInfo($index);
		}
	}

	public function getInfo($id) {
		$jsoup = new JsoupScript;
		$jsoup->url = 'http://minecraft.curseforge.com/projects/' . $id . '/files';

		$jsoup->on('error', function(ScriptEvent $event = null) {
			$GLOBALS['getInfo.error']++;

			$this->form("MainForm")->showPreloader("Загружено: {$GLOBALS['getInfo.parse']} / {$GLOBALS['list.count']}\nОшибок: {$GLOBALS['getInfo.error']}");

			if ($GLOBALS['getInfo.error'] + $GLOBALS['getInfo.parse'] == $GLOBALS['list.count']) {
				$this->form("MainForm")->hidePreloader();
			}
		});

		$jsoup->on('parse', function(ScriptEvent $event = null) {
			$id = explode('/', $event->sender->url)[4];

			$versions = $this->parseVersions($event->sender->find('select#filter-game-version'));

			$GLOBALS['list.mod'][$id]['type']            = $this->parseTypes($event->sender->findFirst('h2.RootGameCategory')->text());
			$GLOBALS['list.mod'][$id]['name']            = $event->sender->findFirst('span.overflow-tip')->text();
			$GLOBALS['list.mod'][$id]['version']         = $versions;
			$GLOBALS['list.mod'][$id]['img.url']         = explode('"', $event->sender->findFirst('a.e-avatar64')->html() )[1];
			$GLOBALS['list.mod'][$id]['img.file']        = $this->createImage( $GLOBALS['list.mod'][$id]['img.url'] );
			$GLOBALS['list.mod'][$id]['file.notRealURL'] = "https://minecraft.curseforge.com" . $event->sender->findFirst('a.overflow-tip')->attr("href");

			$this->getAvailableVersions($versions);

			$this->getRealFileName($id);
		});

		$jsoup->parseAsync();
	}

	public function parseTypes($types) {
		$replaces = [
			'Mods' => 'Мод',
			'Texture Packs' => 'Текстур-пак'
		];

		foreach ($replaces as $what => $to) {
			$types = str::replace($types, $what, $to);
		}

		return $types;
	}

	public function parseVersions($versions_notParsed) {
		$_versions_notParsed = explode('</option>', $versions_notParsed->html() );

		for ($i = 0; $i < count($_versions_notParsed) - 1; $i++) {
			if (Regex::match('Minecraft', $_versions_notParsed[$i]) or Regex::match('All', $_versions_notParsed[$i])) {
				continue;
			} elseif (Regex::match('Java', $_versions_notParsed[$i])) {
				break;
			}

			$version = substr(explode('>', $_versions_notParsed[$i])[1], 12);
			$code = explode('"', $_versions_notParsed[$i])[1];

			$versions[] = $version;
			$codes[$version] = $code;
		}
		$GLOBALS['versions.codes'] = array_merge($GLOBALS['versions.codes'], $codes);

		return $versions;
	}

	public function getAvailableVersions($versions) {
		if(!isset($GLOBALS['versions.available'])) {
			$GLOBALS['versions.available'] = $versions;
		} else {
			$index = array_flip($GLOBALS['versions.available']);
			foreach ($versions as $value) {
				if (isset($index[$value])) {
					unset($index[$value]);
				}
			}
			foreach ($index as $value => $key) {
				unset($GLOBALS['versions.available'][$key]);
			}
			$GLOBALS['versions.available'] = arr::sort($GLOBALS['versions.available']);
		}

		$this->combobox->items->setAll($GLOBALS['versions.available']);
	}

	public function createImage($img) {
		$View = new UXImageArea();
		$View->size = [32, 32];
		$View->centered = true;
		$View->proportional = true;
		$View->stretch = true;
		$View->smartStretch = true;
		$View->image = UXImage::ofUrl($img);
		return $View;
	}

	public function getRealFileName($id) {
		$jsoup = new JsoupScript;
		$jsoup->url = $GLOBALS['list.mod'][$id]['file.notRealURL'];

		$jsoup->on('error', function(ScriptEvent $event = null) {
			$id = explode('/', $event->sender->url)[4];

			$GLOBALS['list.mod'][$id]['file.name'] = null;
		});

		$jsoup->on('parse', function(ScriptEvent $event = null) {
			$id = explode('/', $event->sender->url)[4];

			$GLOBALS['list.mod'][$id]['file.name'] = $event->sender->findFirst('div.info-data')->text();
			$GLOBALS['list.mod'][$id]['file.name'] = str::replace($GLOBALS['list.mod'][$id]['file.name'], " ", "%20");

			$this->getRealFileURL($id);
		});

		$jsoup->parseAsync();
	}

	public function getRealFileURL($id) {
		$url = explode('/', $GLOBALS['list.mod'][$id]['file.notRealURL']);

		unset($GLOBALS['list.mod'][$id]['file.notRealURL']);

		$firstCode  = (int) substr($url[6], 0, 4);
		$secondCode = (int) substr($url[6], 4);

		$GLOBALS['list.mod'][$id]['file.url'] = "https://addons-origin.cursecdn.com/files/$firstCode/$secondCode/{$GLOBALS['list.mod'][$id]['file.name']}$add";

		$this->addItem($id);
	}

	public function addItem($id) {
		$ver = $GLOBALS['list.mod'][$id]['version'];

		$GLOBALS['getInfo.parse']++;

		$item = [
			$GLOBALS['list.mod'][$id]['name'],
			$GLOBALS['list.mod'][$id]['type'] . " - " . $ver[count($ver) - 1] . " / " . $ver[0],
			$GLOBALS['list.mod'][$id]['img.file']
		];

		$this->modList->items->add($item);
		$this->form("MainForm")->showPreloader("Загружено: {$GLOBALS['getInfo.parse']} / {$GLOBALS['list.count']}\nОшибок: {$GLOBALS['getInfo.error']}");

		if ($GLOBALS['getInfo.error'] + $GLOBALS['getInfo.parse'] == $GLOBALS['list.count']) {
			$this->form("MainForm")->hidePreloader();
		}
	}

	/**
	* @event downloader.progress
	*/
	function doDownloaderProgress(ScriptEvent $e = null) {
		$text = "Загружено {$GLOBALS['mod.downloaded']} / {$GLOBALS['list.count']}\n";
		$text .= "Скорость: " . number_format($this->downloader->speed / 1024 / 1024, 2, ".", "") . " Мб/с";

		$this->form("MainForm")->showPreloader($text);            
	}

	/**
	* @event downloader.done
	*/
	function doDownloaderDone(ScriptEvent $e = null) {
		$this->form("MainForm")->hidePreloader();
	}

	/**
	* @event downloader.errorOne
	*/
	function doDownloaderErrorOne(ScriptEvent $e = null) {
		$message = $e->error ?: 'Неизвестная ошибка';

		if ($e->response->isNotFound()) {
			$message = 'Файл не найден';
		} else if ($e->response->isAccessDenied()) {
			$message = 'Доступ запрещен';
		} else if ($e->response->isServerError()) {
			$message = 'Сервер недоступен';
		}

		UXDialog::show('Ошибка загрузки файла: ' . $message, 'ERROR');
	}

	/**
	* @event downloader.successOne
	*/
	function doDownloaderSuccessOne(ScriptEvent $e = null) {
		$GLOBALS['mod.downloaded']++;
	}

	public function createMenuItem($name, $imagePath, $functionAction) { 
		$itemImage = new UXImageView(); 
		$itemImage->image = new UXImage($imagePath); 

		$item = new UXMenuItem($name, $itemImage); 
		$item->on('action', $functionAction); 

		return $item; 
	} 

	public function createMenu($name, $items) { 
		$menu = new UXMenu($name); 

		foreach ($items as $value) { 
			$menu->items->add($value); 
		} 

		return $menu; 
	} 

	public function createMenuBar() { 
        // Menu "Build"          
		$itemsBuild[] = $this->createMenuItem("Создать сборку", "res://.data/img/add.png", function () { 
			$this->chooserFileSave->execute(); 
		}); 

		$itemsBuild[] = $this->createMenuItem("Открыть сборку", "res://.data/img/open.png", function () { 
			$this->chooserFileOpen->execute(); 
		}); 

		$itemsBuild[] = $this->createMenuItem("Скачать сборку", "res://.data/img/ok.png", function () { 
			if(!$GLOBALS['build.opened']) { 
				UXDialog::showAndWait("Сначала откройте или создайте сборку!", 'WARNING'); 
				return; 
			} 
			$this->showPreloader("Загрузка"); 

			$this->chooserDownload->execute(); 
			$folder = $this->chooserDownload->file; 

			$list = []; 
			foreach ($GLOBALS['list.mod'] as $element) { 
				$list[] = $element['file.url']; 
			} 

			$this->downloader->useTempFile = true; 
			$this->downloader->destDirectory = $folder; 
			$this->downloader->urls = $list; 
			$this->downloader->start(); 
		}); 

		$GLOBALS['formMain_menuBar_menuBuild'] = $this->createMenu("Сборка", $itemsBuild); 

        // Menu "Mod"          
		$itemsMod[] = $this->createMenuItem("Поиск в браузере", "res://.data/img/add.png", function () { 
			$this->form("Browser")->show(); 
		}); 

		$GLOBALS['formMain_menuBar_menuMod'] = $this->createMenu("Мод", $itemsMod); 

        // Menu "Help"          
		$itemsHelp[] = $this->createMenuItem("О программе", "res://.data/img/add.png", function () { 
			$this->form("About")->showAndWait(); 
		}); 

		$GLOBALS['formMain_menuBar_menuHelp'] = $this->createMenu("Помощь", $itemsHelp); 

        // Menu bar 
		$GLOBALS['formMain_menuBar'] = new UXMenuBar(); 
		$GLOBALS['formMain_menuBar']->width = $this->form("MainForm")->width; 

		$GLOBALS['formMain_menuBar']->menus->add($GLOBALS['formMain_menuBar_menuBuild']); 
		$GLOBALS['formMain_menuBar']->menus->add($GLOBALS['formMain_menuBar_menuMod']); 
		$GLOBALS['formMain_menuBar']->menus->add($GLOBALS['formMain_menuBar_menuHelp']); 

        // Form 
		$this->form("MainForm")->add($GLOBALS['formMain_menuBar']); 
	}
}