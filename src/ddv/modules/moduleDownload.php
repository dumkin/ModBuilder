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
    
        $text = "Загружено {$GLOBALS["download"]["file"]["success"]} / {$GLOBALS["download"]["file"]["all"]}\n" .
                "Скорость: {$speed} Мб/с";

        app()->form("MainForm")->showPreloader($text);           
    }

    /**
     * @event downloader.successOne 
     */
    function doDownloaderSuccessOne(ScriptEvent $e = null) {
        $GLOBALS["download"]["file"]["success"]++;
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
    
    
    public function downloadStart($version) {
        app()->form("MainForm")->showPreloader("Выберите папку для скачивания модов");
    
        $directoryChooser = new DirectoryChooserScript;
        
        if (!$directoryChooser->execute()) {
            app()->form("MainForm")->hidePreloader();
            return;
        }
        
        app()->form("MainForm")->showPreloader("Подготовка к скачиванию модов"); 
        
        $this->downloader->destDirectory = $directoryChooser->file;

        $GLOBALS["download"]["prepare"]["all"] = $GLOBALS["project"]["mods"]["count"];
        $GLOBALS["download"]["prepare"]["success"] = 0;
        $GLOBALS["download"]["prepare"]["error"] = 0;
        
        foreach ($GLOBALS["project"]["mods"]["list"] as $id => $element) {
            $this->parseRealLatestFile('http://minecraft.curseforge.com/projects/' . $id . '/files?filter-game-version=' . $GLOBALS['versions.codes'][$version]);
        }
    }
        
    public function parseRealLatestFile($url) {
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
        
                $GLOBALS["download"]["list"][] = "https://addons-origin.cursecdn.com/files/$firstCode/$secondCode/{$fileName}";
                
                $GLOBALS["download"]["prepare"]["success"]++;
            
                $text = "Подготовлено: {$GLOBALS["download"]["prepare"]["success"]} / {$GLOBALS["download"]["prepare"]["all"]}";
                if ($GLOBALS["download"]["prepare"]["error"] != 0) {
                    $text .= "\nОшибок: {$GLOBALS["download"]["prepare"]["error"]}";
                }
                
                $this->form("MainForm")->showPreloader($text);
    
                if ($GLOBALS["download"]["prepare"]["success"] + $GLOBALS["download"]["prepare"]["error"] == $GLOBALS["download"]["prepare"]["all"]) {
                    $GLOBALS["download"]["file"]["all"] = $GLOBALS["project"]["mods"]["count"];
                    $GLOBALS["download"]["file"]["success"] = 0;
                    $GLOBALS["download"]["file"]["error"] = 0;
                
                    $this->downloader->urls = $GLOBALS["download"]["list"]; 
                    $this->downloader->start();
                }
            });
    
            $jsoup->parseAsync();
        });

        $jsoup->parseAsync();
    }
}