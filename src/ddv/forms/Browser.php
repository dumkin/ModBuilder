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
        if(!$GLOBALS["project"]["opened"]) {
            UXDialog::showAndWait("Сначала откройте или создайте сборку!", "WARNING");
            return;
        }

        $this->showPreloader("Поиск мода");

        $name = explode('/', $this->browser->url)[4];
        if (!isset($name) and $name == "") {
            $this->hidePreloader();
            UXDialog::showAndWait("Мод не найден!", 'WARNING');
            return;
        }

        foreach ($GLOBALS["project"]["mods"]["list"] as $id => $value) {
            if($id == $name) {
                UXDialog::showAndWait("Мод уже был добавлен в сборку!", 'WARNING');
                $this->hidePreloader();
                return;
            }
        }

        $jsoup = new JsoupScript;
        $jsoup->url = $this->browser->url;
        
        $jsoup->on('parse', function(ScriptEvent $event = null) {
            $this->form("MainForm")->showPreloader("Получение информации о модах");
            
            $id = explode('/', $this->browser->url)[4];
            $id = explode('?', $id)[0];
            
            $GLOBALS["project"]["mods"]["count"]++;
            $GLOBALS["modification"]["parsing"]["all"]++;
            $GLOBALS["project"]["mods"]["list"][$id] = [];
            
            $ids = array_keys($GLOBALS["project"]["mods"]["list"]);
            $ids = implode("\n", $ids);
            
            Stream::putContents($GLOBALS["project"]["file"], $ids);

            app()->module("moduleModification")->parseId($id);
            
            $name = $event->sender->findFirst('.overflow-tip')->text();
            
            $type = $event->sender->findFirst('h2.RootGameCategory')->text();
            $type = app()->module("moduleModification")->parseTypes($type);
            
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