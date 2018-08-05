import { ErrorHandler, Injector, Injectable} from '@angular/core';
import {LoggingService} from './logging.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private injector: Injector) { }

  handleError(error: any) {
     const loggingService = this.injector.get(LoggingService);
     
     const message = error.message ? error.message : error.toString();          

     loggingService.logError(message, error);
          
     // IMPORTANT: Rethrow the error otherwise it gets swallowed
     throw error;
  }  
}