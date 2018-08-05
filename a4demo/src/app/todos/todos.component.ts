import { Component } from '@angular/core';
import {TodosService} from './todos.service';
import {Todo} from './todo';

import {LoggingService} from '../services/logging.service';

@Component({
    templateUrl: 'todos.component.html',
    providers: [TodosService]
})
export class TodosComponent {
    constructor(private todoSvc: TodosService, private loggingService: LoggingService) {  }
    todoList: Todo[];
    loadingTodos: boolean;
    singleTodo: Todo;
    newTodoText: string;
    updatedMessage: string;
    failedMessage: string;
    loadingAllError: string;

    getAllTodos() {
        this.loadingTodos = true;
        this.loadingAllError = "";
        this.loggingService.startPerfTracker("GETTING_TODOS", null);
        this.todoSvc.getTodos().subscribe(
            todoReponse => { 
                this.todoList = <Todo[]>todoReponse; 
                this.loadingTodos = false;
                this.loggingService.stopPerfTracker("GETTING_TODOS");
            },
            errorResponse => {
                this.loadingAllError = errorResponse;             
                this.loadingTodos = false;
            }
        )
    }

    getTodo2() {        
        this.todoSvc.getSingleTodo(2).subscribe(
            todoReponse => { 
                this.singleTodo = <Todo>todoReponse; 
            },
            errorResponse => {
                // todo show error                
            }
        )
    }

    updateTodo2() {
        var todo = new Todo();
        todo.Id = 2;
        todo.Item = this.newTodoText;
        todo.Completed = false;

        this.updatedMessage = "";
        this.failedMessage = "";

        this.todoSvc.updateTodo(todo).subscribe(
            good => {
                this.updatedMessage = "Todo 2 updated successfully."
            },
            bad => {
                this.failedMessage = "Something failed with the update of the todo!";
            }
        )
    }
}