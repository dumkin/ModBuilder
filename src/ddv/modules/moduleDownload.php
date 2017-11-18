<?php
namespace ddv\modules;

use std, gui, framework, ddv;
use script\DirectoryChooserScript;
use script\JsoupScript;

class moduleDownload extends AbstractModule {
    /**
     * @event downloader.done 
     */
    function doDownloaderDone(ScriptEvent $e = null) {
        app()->form("MainForm")->hidePreloader();
    }

    /**
     * @event downloader.successAll 
     */
    function doDownloaderSuccessAll(ScriptEvent $e = null) {
        
    }

    /**
     * @event downloader.progress 
     */
    function doDownloaderProgress(ScriptEvent $e = null) {
        $speed = number_format($this->downloader->speed / 1024 / 1024, 2, ".", "");
    
        $text = "Загружено {$GLOBALS['mod.downloaded']} / {$GLOBALS['list.count']}\n" .
                "Скорость: {$speed} Мб/с";

        app()->form("MainForm")->showPreloader($text);           
    }

    /**
     * @event downloader.successOne 
     */
    function doDownloaderSuccessOne(ScriptEvent $e = null) {
        $GLOBALS['mod.downloaded']++;
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
    
    
    public function modsDownload($version) {
        $directoryChooser = new DirectoryChooserScript;
        $directoryChooser->execute(); 
        
        $this->downloader->destDirectory = $directoryChooser->file;

        $GLOBALS['download.progress'] = 0;
        $GLOBALS['download.progress.max'] = count($GLOBALS['list.mod']);
        
        foreach ($GLOBALS['list.mod'] as $key => $element) {
            $this->getRealFileURL('http://minecraft.curseforge.com/projects/' . $key . '/files?filter-game-version=' . $GLOBALS['versions.codes'][$version]);
        }
    }
    
    public function modsDownloadStart() {
        $this->downloader->urls = $GLOBALS['download.list.url']; 
        $this->downloader->start(); 
    }
    
    public function getRealFileURL($url) {
        $jsoup = new JsoupScript;
        $jsoup->url = $url;

        $jsoup->on('parse', function(ScriptEvent $event = null) {
            $jsoup = new JsoupScript;
            $jsoup->url = "https://minecraft.curseforge.com" . $event->sender->findFirst('a.overflow-tip')->attr("href");

            $jsoup->on('parse', function(ScriptEvent $event = null) {
                $url = explode('/', $event->sender->url);

                $firstCode  = (int) substr($url[6], 0, 4);
                $secondCode = (int) substr($url[6], 4);
                
                $fileName = $event->sender->findFirst('div.info-data')->text(); 
                $fileName = str::replace($fileName, " ", "%20"); 
        
                $GLOBALS['download.list.url'][] = "https://addons-origin.cursecdn.com/files/$firstCode/$secondCode/{$fileName}$add";

                $GLOBALS['download.progress']++;
                app()->form("MainForm")->showPreloader("Подготовлено: {$GLOBALS['download.progress']} / {$GLOBALS['download.progress.max']}"); 
                if ($GLOBALS['download.progress.max'] == $GLOBALS['download.progress']) {
                    $this->modsDownloadStart();
                }
            });
    
            $jsoup->parseAsync();
        });

        $jsoup->parseAsync();
    }
}