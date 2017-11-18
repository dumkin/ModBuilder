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

            $this->getAvailableVersions($versions);

            $this->addItem($id);
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
            
            if($this->combobox->selectedIndex == -1) { 
                UXDialog::showAndWait("Сначала выберете версию!", 'WARNING'); 
                return; 
            }
            
            $this->showPreloader("Подготовка к загрузке"); 

            //$this->modsDownload($this->combobox->selected);
            app()->module('moduleDownload')->modsDownload($this->combobox->selected);
            //$this->appModule("moduleDownload")->modsDownload($this->combobox->selected);
           
            //$this->modsDownload($this->combobox->selected);
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
        $GLOBALS['formMain_menuBar']->height = 30;

        $GLOBALS['formMain_menuBar']->menus->add($GLOBALS['formMain_menuBar_menuBuild']); 
        $GLOBALS['formMain_menuBar']->menus->add($GLOBALS['formMain_menuBar_menuMod']); 
        $GLOBALS['formMain_menuBar']->menus->add($GLOBALS['formMain_menuBar_menuHelp']); 

        // Form 
        $this->form("MainForm")->add($GLOBALS['formMain_menuBar']); 
    }
}
