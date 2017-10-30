<?php
namespace ddv\forms;

use std, gui, framework, ddv;
use php\io\Stream;
use script\JsoupScript;

class Browser extends AbstractForm {
    /**
     * @event addMod.action
     */
    function doAddModAction(UXEvent $e = null) {
        if(!$GLOBALS['build.opened']) {
            UXDialog::showAndWait("Сначала откройте или создайте сборку!", 'WARNING');
            return;
        }

        $this->showPreloader("Поиск мода");

        $name = explode('/', $this->browser->url)[4];
        if (!isset($name) and $name == "") {
            $this->hidePreloader();
            UXDialog::showAndWait("Мод не найден!", 'WARNING');
            return;
        }

        foreach ($GLOBALS['list.mod'] as $key => $value) {
            if($key == $name) {
                UXDialog::showAndWait("Мод уже был добавлен в сборку!", 'WARNING');
                $this->hidePreloader();
                return;
            }
        }

        $jsoup = new JsoupScript;
        $jsoup->url = $this->browser->url;
        
        $jsoup->on('parse', function(ScriptEvent $event = null) {
            $type = $event->sender->findFirst('h2.RootGameCategory')->text();
            $name = $event->sender->findFirst('.overflow-tip')->text();
            $id = explode('/', $this->browser->url)[4];
            
            $type = $this->form("MainForm")->parseTypes($type);
        
            Stream::putContents($GLOBALS['build.file'], "\n" . $id, "a+");
            
            $this->form("MainForm")->showPreloader("Загрузка данных");
            $GLOBALS['list.count']++;
            $this->form("MainForm")->getInfo($id);
            
            UXDialog::showAndWait("$type $name - добавлен в сборку!");
            $this->hidePreloader();
        });
        
        $jsoup->parseAsync();
    }
    /**
     * @event home.action
     */
    function doHomeAction(UXEvent $e = null) {
        $this->browser->engine->load('http://minecraft.curseforge.com/mc-mods');
    }
    /**
     * @event back.action
     */
    function doBackAction(UXEvent $e = null) {
        if ($this->browser->engine->history->currentIndex == 0) return;
        $this->browser->engine->history->goBack();
    }
    /**
     * @event forward.action
     */
    function doForwardAction(UXEvent $e = null) {
        if($this->browser->engine->history->currentIndex == sizeof($this->browser->engine->history->getEntries()) - 1) return;
        $this->browser->engine->history->goForward();
    }
    /**
     * @event construct 
     */
    function doConstruct(UXEvent $e = null) {    
        $this->browser->engine->load("http://minecraft.curseforge.com/mc-mods");
    }
}