<?php
namespace ddv\forms;

use std, gui, framework, ddv;

class About extends AbstractForm {
    /**
     * @event button.action 
     */
    function doButtonAction(UXEvent $e = null) {    
        $this->hide();
    }
}