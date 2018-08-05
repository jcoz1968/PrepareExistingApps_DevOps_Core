import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';
import { FormsModule }   from '@angular/forms'; 
import { AppComponent } from './app.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { SimpleComponent } from './simple/simple.component';
import { TodosComponent } from './todos/todos.component';
import { SecurityService } from './services/security.service';

import { LoggingService } from './services/logging.service';
import { GlobalErrorHandler } from './services/error.handler';

@NgModule({
  declarations: [
    AppComponent,
    SimpleComponent,
    TodosComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    FormsModule,
    RouterModule.forRoot([    
      {path: "simple", component: SimpleComponent },
      {path: "todos", component: TodosComponent },
      {path: '', redirectTo: '/simple', pathMatch: 'full'}
    ]),    
    NgbModule.forRoot()
  ],  
  providers: [
                SecurityService, 
                LoggingService,  
                { provide: ErrorHandler, useClass: GlobalErrorHandler} 
             ],
  bootstrap: [AppComponent]
})
export class AppModule { }