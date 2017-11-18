<?php
namespace ddv\modules;

use std, gui, framework, ddv;
use script\script\FileChooserScript;

class moduleProject extends AbstractModule {
    public function projectCreate() {
        $this->form("MainForm")->showPreloader("Выберите файл для новой сборки");
    
        $fileChooser = new FileChooserScript;
        $fileChooser->saveDialog = true;
        
        if (!$fileChooser->execute()) {
            $this->form("MainForm")->hidePreloader();
            return;
        }
        
        $this->projectClose();
        
        $GLOBALS["project"]["file"] = $fileChooser->file;
        $GLOBALS["project"]["name"] = fs::nameNoExt($GLOBALS["project"]["file"]);
        $GLOBALS["project"]["opened"] = true;
        $GLOBALS["project"]["mods"]["count"] = 0;
        $GLOBALS["project"]["mods"]["list"] = [];
        $GLOBALS["project"]["versions"]["available"] = null;
        $GLOBALS["project"]["versions"]["all"] = [];
        
        $this->form("MainForm")->title = "Сборка - {$GLOBALS["project"]["name"]}";

        Stream::putContents($GLOBALS["project"]["file"], "");
        
        $this->form("MainForm")->hidePreloader();
    }
    
    public function projectOpen() {
        $this->form("MainForm")->showPreloader("Выберите файл сборки");
    
        $fileChooser = new FileChooserScript;
        $fileChooser->filterExtensions = "*";
        
        if (!$fileChooser->execute()) {
            $this->form("MainForm")->hidePreloader();
            return;
        }
        
        $this->projectClose();
        
        $GLOBALS["project"]["file"] = $fileChooser->file;
        $GLOBALS["project"]["name"] = fs::nameNoExt($GLOBALS["project"]["file"]);
        $GLOBALS["project"]["opened"] = true;
        $GLOBALS["project"]["mods"]["count"] = 0;
        $GLOBALS["project"]["mods"]["list"] = [];
        $GLOBALS["project"]["versions"]["available"] = null;
        $GLOBALS["project"]["versions"]["all"] = [];
        
        $this->form("MainForm")->title = "Сборка - {$GLOBALS["project"]["name"]}";
        
        $stream = Stream::of($GLOBALS["project"]["name"]);
        $scanner = new Scanner($stream);

        while ($scanner->hasNextLine()) {
            $GLOBALS["project"]["mods"]["count"]++;

            $id = $scanner->nextLine();
            $GLOBALS["project"]["mods"]["list"][$id] = [];
        }

        $stream->close();
        
        $this->form("MainForm")->showPreloader("Получение информации о модах");

        app()->module("moduleModification")->parseClear();
        foreach ($GLOBALS["project"]["mods"]["list"] as $id => $element) {
            app()->module("moduleModification")->parseId($id);
        }
    }
    
    public function projectClose() {
        unset($GLOBALS["project"]);
        
        $GLOBALS["project"]["opened"] = false;

        $this->form("MainForm")->modList->items->clear();
        $this->form("MainForm")->title = "Сборка не открыта";
    }
}