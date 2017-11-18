<?php
namespace ddv\forms;

use std, gui, framework, ddv;
use php\desktop\Mouse;
use php\gui\UXDesktop;

class MainForm extends AbstractForm {
    /**
     * @event construct
     */
    function doConstruct(UXEvent $e = null) {
        $this->modList->setCellFactory(function(UXListCell $cell, $item) {
            if ($item) {
                $titleName = new UXLabel($item[0]);
                $titleName->style = '-fx-font-weight: bold;';

                $titleDescription = new UXLabel($item[1]);
                $titleDescription->style = '-fx-text-fill: gray;';

                $title = new UXVBox([$titleName, $titleDescription]);
                $title->spacing = 0;

                $line = new UXHBox([$item[2], $title]);
                $line->spacing = 7;
                $line->padding = 5;
                $cell->text = null;
                $cell->graphic = $line;
            }
        });

        $this->createMenuBar();
    }
    
    /**
     * @event modList.click-Right 
     */
    function doModListClickRight(UXMouseEvent $e = null) {
        if ($this->modList->selectedIndex == -1) {
            return;
        }
        
        $itemDelete = new UXMenuItem('Удалить'); 
        $itemDelete->on('action', function () use ($itemDelete) {
            $id = 0;
            foreach ($GLOBALS["project"]["mods"]["list"] as $index => $element) {
                if ($this->modList->selectedItem[0] == $element["name"]) {
                    $id = $index;
                }
            }
        
            unset($GLOBALS["project"]["mods"]["list"][$id]);
            $this->modList->items->removeByIndex($this->modList->selectedIndex);
                     
            $id = array_keys($GLOBALS["project"]["mods"]["list"]);
            $id = implode("\n", $id);
            
            Stream::putContents($GLOBALS["project"]["file"], $id);
        });
        
        $contextMenu = new UXContextMenu();
        $contextMenu->items->add($itemDelete);   
        
        $offsetX = Mouse::x() - $this->x; 
        $offsetY = Mouse::y() - $this->y - 60;

        $contextMenu->showByNode($this->modList, $offsetX, $offsetY);
    }

    /**
     * @event modList.click-2x 
     */
    function doModListClick2x(UXMouseEvent $e = null) {    
        if ($this->modList->selectedIndex == -1) {
            return;
        }
        
        foreach ($GLOBALS["project"]["mods"]["list"] as $index => $element) {
            if ($this->modList->selectedItem[0] == $element["name"]) {
                $desktop = new UXDesktop();
                $desktop->open('http://minecraft.curseforge.com/projects/' . $index . '/');
                return;
            }
        }
    }
}