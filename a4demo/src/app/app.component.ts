import { Component, Provider } from '@angular/core';
import { HttpModule } from '@angular/http';
import {Router, Event, NavigationEnd, NavigationStart} from '@angular/router';

import {SecurityService} from './services/security.service';

import {LoggingService} from './services/logging.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [SecurityService, HttpModule, LoggingService ]
})
export class AppComponent {
  title = 'app';
  username = "";

  constructor(private securityService: SecurityService, router: Router, 
    private loggingService: LoggingService ) {    

    if (window.location.hash && !this.securityService.isAuthorized) {
      this.securityService.authorizeCallback();      
    } else if (!this.securityService.isAuthorized) {
      this.securityService.authorize();
    }
    securityService.getUserInfo().subscribe(userInfo => this.username = userInfo.given_name);  

    router.events.subscribe((event: Event) => {  
        if (event instanceof NavigationStart && this.securityService.isAuthorized) {
          loggingService.startPerfTracker(event.url, null);
        }

		    if (event instanceof NavigationEnd && this.securityService.isAuthorized)  {
           loggingService.logUsage(event.urlAfterRedirects, {});  
           loggingService.stopPerfTracker(event.urlAfterRedirects);        
         }
       }
     );  
  }
}
