import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';

import { SecurityService } from '../services/security.service';
import {Todo} from './todo';
import { LoggingService } from '../services/logging.service';

@Injectable()
export class TodosService { 
    constructor(private http: Http, private securityService: SecurityService, 
        private loggingService: LoggingService) { }        
    
    getTodos(): Observable<{} | Todo[]> {
        return this.http.get("https://sampleapi.knowyourtoolset.com/api/Todos", 
                { headers: this.securityService.getHeaders() })
            .map((response: Response) => <Todo[]>response.json())            
            .catch(failure => { 
                return this.loggingService.handleApiError("Failed getting todos!", failure); 
            });                         
    }

    getSingleTodo(todoId: number): Observable<{} | Todo> {
        return this.http.get("https://sampleapi.knowyourtoolset.com/api/Todos/" + 
                    todoId.toString(), { headers: this.securityService.getHeaders() })
            .map((response: Response) => <Todo>response.json())            
            .catch(failure => { 
                return this.loggingService.handleApiError(
                    "Error occurred calling getTodo for todo [" 
                    + todoId.toString() + "]!", failure); 
            });            
    }

    updateTodo(todoToUpate: Todo): Observable<{} | string> {
        let headers = this.securityService.getHeaders();
        headers.append("Content-Type", "application/json");
        return this.http.put("https://sampleapi.knowyourtoolset.com/api/Todos/" + 
                    todoToUpate.Id.toString(), JSON.stringify(todoToUpate), { headers: headers })
            .map((response: Response) => <string>"Success!")            
            .catch(failure => { 
                failure.todoId = todoToUpate.Id;
                failure.todoItem = todoToUpate.Item;
                failure.todoCompleted = todoToUpate.Completed;
                return this.loggingService.handleApiError(
                    "Error occurred updating Todo for todo [" 
                    + todoToUpate.Id.toString() + "]!", failure); });                  
    }
}       