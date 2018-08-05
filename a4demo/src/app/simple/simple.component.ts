import { Component } from '@angular/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {LoggingService} from '../services/logging.service';

@Component({
    templateUrl: 'simple.component.html'    
})
export class SimpleComponent {
    constructor(private loggingService: LoggingService) { }

    public simpleClick() {
        this.loggingService.logDiagnostic("Button Clicked", { moreStuff: "crazy wicked cool!"});

        alert('clicked!');
        throw new Error('bad news!');
    }
}