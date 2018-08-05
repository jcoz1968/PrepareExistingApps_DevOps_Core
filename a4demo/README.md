# A4demo

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 1.1.3.  The instructions and guidance for simply getting started are here:
[https://angular.io/guide/quickstart](https://angular.io/guide/quickstart)

Then I added both bootstrap v4 and the ng-bootstrap packages (`npm install XXX --save`):
   
    "bootstrap": "^4.0.0-alpha.6",
    "@ng-bootstrap/ng-bootstrap": "^1.0.0-alpha.26",

## Key features
This demo uses / showcases the following:
* Two core functional components:
    - a `simple` component with a clickable button
    - a `todos` component that has multiple buttons used to make API calls
* Requires authenticated users -- authentication is done through a demo version of `IdentityServer4` found [here](https://demo.identityserver.io).

# Logging goals
When we get to the logging, we'll be after the following:
* Build a `service` to handle all logging requests
* Hook into the Router to track completed navigation events as `Usage` events, and difference between nav start and nav end with a `performance` entry
* Globally handle errors from Angular 
* Handle API errors with special logic for `CorrelationId`

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `-prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).
Before running the tests make sure you are serving the app via `ng serve`.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).
