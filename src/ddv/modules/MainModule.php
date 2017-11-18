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
            app()->module('moduleProject')->projectCreate();
        });

        $itemsBuild[] = $this->createMenuItem("Открыть сборку", "res://.data/img/open.png", function () { 
            app()->module('moduleProject')->projectOpen(); 
        });
        
        $itemsBuild[] = $this->createMenuItem("Закрыть сборку", "res://.data/img/open.png", function () { 
            app()->module('moduleProject')->projectClose();
        });

        $itemsBuild[] = $this->createMenuItem("Скачать сборку", "res://.data/img/ok.png", function () { 
            if(!$GLOBALS["project"]["opened"]) { 
                UXDialog::showAndWait("Сначала откройте или создайте сборку!", 'WARNING'); 
                return; 
            }
            
            if($this->combobox->selectedIndex == -1) { 
                UXDialog::showAndWait("Сначала выберете версию!", 'WARNING'); 
                return; 
            }
            
            $this->showPreloader("Подготовка к загрузке"); 

            app()->module('moduleDownload')->downloadStart($this->combobox->selected);
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
