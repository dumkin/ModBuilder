<?php
namespace ddv\forms;

use std, gui, framework, ddv;
use php\desktop\Mouse; 

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

        $mainMenu = new UXMenuBar();
        $mainMenu->width = 800;
        $this->add($mainMenu);

        $GLOBALS['Menu_Menu'] = new UXMenu('Меню');

        $create = new UXImageView();
        $create->image = new UXImage('res://.data/img/add.png');
        $itemCreate = new UXMenuItem('Создать сборку', $create);
        $itemCreate->on('action', function ($e) use ($itemCreate) {
            $this->chooserFileSave->execute();
        });

        $open = new UXImageView();
        $open->image = new UXImage('res://.data/img/open.png');
        $itemOpen = new UXMenuItem('Открыть сборку', $open);
        $itemOpen->on('action', function ($e) use ($itemOpen) {
           $this->chooserFileOpen->execute();
        });

        $download = new UXImageView();
        $download->image = new UXImage('res://.data/img/ok.png');
        $itemDownload = new UXMenuItem('Загрузить сборку', $download);
        $itemDownload->on('action', function ($e) use ($itemDownload) {
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

        $GLOBALS['Menu_Mod'] = new UXMenu('Мод');

        $search = new UXImageView();
        $search->image = new UXImage('res://.data/img/add.png');
        $itemSearch = new UXMenuItem('Поиск в браузере', $search);
        $itemSearch->on('action', function ($e) use ($itemSearch) {
            $this->form("Browser")->show();
        });
        
        $GLOBALS['Menu_About'] = new UXMenu('О программе');

        $about = new UXImageView();
        $about->image = new UXImage('res://.data/img/add.png');
        $itemAbout = new UXMenuItem('Поиск в браузере', $about);
        $itemAbout->on('action', function ($e) use ($itemAbout) {
            $this->form("About")->showAndWait();
        });

        $mainMenu->menus->add($GLOBALS['Menu_Menu']);
        $mainMenu->menus->add($GLOBALS['Menu_Mod']);
        $mainMenu->menus->add($GLOBALS['Menu_About']);

        $GLOBALS['Menu_Menu']->items->add($itemCreate);
        $GLOBALS['Menu_Menu']->items->add($itemOpen);
        $GLOBALS['Menu_Menu']->items->add($itemDownload);
        $GLOBALS['Menu_Mod']->items->add($itemSearch);
        $GLOBALS['Menu_About']->items->add($itemAbout);
    }
    /**
     * @event button.action
     */
    function doButtonAction(UXEvent $e = null) {
        //var_dump($GLOBALS['list.mod']);
        //var_dump($GLOBALS['versions.available']);
        var_dump( $GLOBALS['versions.codes'] );
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
            $this->modList->items->removeByIndex($this->modList->selectedIndex);
        });
        
        $contextMenu = new UXContextMenu();
        $contextMenu->items->add($itemDelete);   
        
        $offsetX = Mouse::x() - $this->x; 
        $offsetY = Mouse::y() - $this->y - 60;

        $contextMenu->showByNode($this->modList, $offsetX, $offsetY);
    }
}