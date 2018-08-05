import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/of'
import { Observable } from 'rxjs/Observable';


@Injectable()
export class SecurityService {    
    private storage: any;

    constructor(private http: Http) {
        this.storage = sessionStorage;

        if (this.retrieve("IsAuthorized") !== "") {            
            this.isAuthorized = this.retrieve("IsAuthorized");
        } else {
            this.isAuthorized = false;
        }
    }

    public isAuthorized: boolean;    

    public getToken(): any {
        return this.retrieve("authorizationData");
    }

    public getHeaders(): Headers {
        var headers = new Headers();
        var token = this.getToken();
        headers.append('Authorization', 'Bearer ' + token);
        return headers;
    }

    public resetAuthorizationData() {
        this.store("authorizationData", "");
        this.store("authorizationDataIdToken", "");

        this.isAuthorized = false;
        this.store("IsAuthorized", false);
    }

    public setAuthorizationData(token: any, id_token: any) {
        if (this.retrieve("authorizationData") !== "") {
            this.store("authorizationData", "");
        }

        this.store("authorizationData", token);
        this.store("authorizationDataIdToken", id_token);
        this.isAuthorized = true;
        this.store("IsAuthorized", true);   
    }    

    public authorize() {
        this.resetAuthorizationData();

        var authorizationUrl = 'https://demo.identityserver.io/connect/authorize';
        var client_id = 'implicit';
        var redirect_uri = 'http://localhost:4200/';
        var response_type = "id_token token";
        var scope = "openid profile email api";
        var nonce = "N" + Math.random() + "" + Date.now();
        var state = Date.now() + "" + Math.random();

        this.store("authStateControl", state);
        this.store("authNonce", nonce);

        var url =
            authorizationUrl + "?" +
            "response_type=" + encodeURI(response_type) + "&" +
            "client_id=" + encodeURI(client_id) + "&" +
            "redirect_uri=" + encodeURI(redirect_uri) + "&" +
            "scope=" + encodeURI(scope) + "&" +
            "nonce=" + encodeURI(nonce) + "&" +
            "state=" + encodeURI(state);

        window.location.href = url;
    }

    public authorizeCallback() {
        this.resetAuthorizationData();

        var hash = window.location.hash.substr(1);

        var result: any = hash.split('&').reduce(function (result, item) {
            var parts = item.split('=');
            result[parts[0]] = parts[1];
            return result;
        }, {});

        var token = "";
        var id_token = "";
        var authResponseIsValid = false;
        if (!result.error) {

            if (result.state !== this.retrieve("authStateControl")) {
                console.log("AuthorizedCallback incorrect state");
            } else {

                token = result.access_token;
                id_token = result.id_token;
                

                var dataIdToken: any = this.getDataFromToken(id_token);

                // validate nonce
                if (dataIdToken.nonce !== this.retrieve("authNonce")) {
                    console.log("AuthorizedCallback incorrect nonce");
                } else {
                    this.store("authNonce", "");
                    this.store("authStateControl", "");

                    authResponseIsValid = true;
                    console.log("AuthorizedCallback state and nonce validated, returning access token");
                }
            }
        }

        if (authResponseIsValid) {
            this.setAuthorizationData(token, id_token);
            this.getUserInfo().subscribe(userData => this.store("userinfo", userData));                                  
        }
        else {
            this.resetAuthorizationData();            
        }
    }

    public getUserInfo(): Observable<any> {
                
        var ui = this.retrieve("userinfo");
        if (ui != null)
            return Observable.of(ui);            

        return this.http.get("https://demo.identityserver.io/connect/userinfo", 
            { headers: this.getHeaders() })
            .map((response: Response) => <any>response.json())            
    }    

    public Logoff() {
        this.resetAuthorizationData();        
    }

    public HandleError(error: any) {
        console.log(error);
        if (error.status === 403) {
            //this._rou.navigate(['Forbidden'])
        }
        else if (error.status === 401) {
            this.resetAuthorizationData();
            //this._router.navigate(['Unauthorized'])
        }
    }

    private urlBase64Decode(str) {
        var output = str.replace('-', '+').replace('_', '/');
        switch (output.length % 4) {
            case 0:
                break;
            case 2:
                output += '==';
                break;
            case 3:
                output += '=';
                break;
            default:
                throw 'Illegal base64url string!';
        }

        return window.atob(output);
    }

    private getDataFromToken(token) {
        var data = {};
        if (typeof token !== 'undefined' && token != null) {
            var encoded = token.split('.')[1];
            data = JSON.parse(this.urlBase64Decode(encoded));
        }

        return data;
    }

    private retrieve(key: string): any {
        var item = this.storage.getItem(key);

        if (item && item !== 'undefined') {
            return JSON.parse(this.storage.getItem(key));
        }

        return null;
    }

    private store(key: string, value: any) {
        this.storage.setItem(key, JSON.stringify(value));
    }

}