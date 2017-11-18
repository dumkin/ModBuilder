<?php
namespace ddv\modules;

use std, gui, framework, ddv;
use script\JsoupScript;

class moduleModification extends AbstractModule {
    public function parseClear() {
        $GLOBALS["modification"]["parsing"]["all"] = $GLOBALS["project"]["mods"]["count"];
        $GLOBALS["modification"]["parsing"]["success"] = 0;
        $GLOBALS["modification"]["parsing"]["error"] = 0;
    }

    public function parseId($id) {
        $jsoup = new JsoupScript;
        $jsoup->url = 'http://minecraft.curseforge.com/projects/' . $id . '/files';

        $jsoup->on('error', function(ScriptEvent $event = null) {
            $GLOBALS["modification"]["parsing"]["success"]++;
            
            $text = "Загружено: {$GLOBALS["modification"]["parsing"]["success"]} / {$GLOBALS["modification"]["parsing"]["all"]}";
            if ($GLOBALS["modification"]["parsing"]["error"] != 0) {
                $text .= "\nОшибок: {$GLOBALS["modification"]["parsing"]["error"]}";
            }

            $this->form("MainForm")->showPreloader($text);

            if ($GLOBALS["modification"]["parsing"]["success"] + $GLOBALS["modification"]["parsing"]["error"] == $GLOBALS["modification"]["parsing"]["all"]) {
                $this->form("MainForm")->hidePreloader();
            }
        });

        $jsoup->on('parse', function(ScriptEvent $event = null) {
            $id = explode('/', $event->sender->url)[4];

            $versions = $this->parseVersions($event->sender->find('select#filter-game-version'));

            $GLOBALS["project"]["mods"]["list"][$id]['type']     = $this->parseTypes($event->sender->findFirst('h2.RootGameCategory')->text());
            $GLOBALS["project"]["mods"]["list"][$id]['name']     = $event->sender->findFirst('span.overflow-tip')->text();
            $GLOBALS["project"]["mods"]["list"][$id]['version']  = $versions;
            $GLOBALS["project"]["mods"]["list"][$id]['img.url']  = explode('"', $event->sender->findFirst('a.e-avatar64')->html() )[1];
            $GLOBALS["project"]["mods"]["list"][$id]['img.file'] = $this->createImage($GLOBALS["project"]["mods"]["list"][$id]['img.url']);

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
        $GLOBALS["project"]["versions"]["all"] = array_merge($GLOBALS["project"]["versions"]["all"], $codes);

        return $versions;
    }

    public function getAvailableVersions($versions) {
        if($GLOBALS["project"]["versions"]["available"] == null) {
            $GLOBALS["project"]["versions"]["available"] = $versions;
        } else {
            $index = array_flip($GLOBALS["project"]["versions"]["available"]);
            foreach ($versions as $value) {
                if (isset($index[$value])) {
                    unset($index[$value]);
                }
            }
            foreach ($index as $value => $key) {
                unset($GLOBALS["project"]["versions"]["available"][$key]);
            }
            $GLOBALS["project"]["versions"]["available"] = arr::sort($GLOBALS["project"]["versions"]["available"]);
        }

        app()->form("MainForm")->combobox->items->setAll($GLOBALS["project"]["versions"]["available"]);
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
        $ver = $GLOBALS["project"]["mods"]["list"][$id]['version'];

        $GLOBALS['getInfo.parse']++;

        $item = [
            $GLOBALS["project"]["mods"]["list"][$id]['name'],
            $GLOBALS["project"]["mods"]["list"][$id]['type'] . " - " . $ver[count($ver) - 1] . " / " . $ver[0],
            $GLOBALS["project"]["mods"]["list"][$id]['img.file']
        ];

        app()->form("MainForm")->modList->items->add($item);
        
        $GLOBALS["modification"]["parsing"]["success"]++;
            
        $text = "Загружено: {$GLOBALS["modification"]["parsing"]["success"]} / {$GLOBALS["modification"]["parsing"]["all"]}";
        if ($GLOBALS["modification"]["parsing"]["error"] != 0) {
            $text .= "\nОшибок: {$GLOBALS["modification"]["parsing"]["error"]}";
        }

        $this->form("MainForm")->showPreloader($text);

        if ($GLOBALS["modification"]["parsing"]["success"] + $GLOBALS["modification"]["parsing"]["error"] == $GLOBALS["modification"]["parsing"]["all"]) {
            $this->form("MainForm")->hidePreloader();
        }
    }
}