import { Component } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs";
import { map } from 'rxjs/operators';
import {NgForm} from '@angular/forms';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent{ 
    constructor(private http: HttpClient) {
       
    }

    onSubmit(f: NgForm) {
        debugger;

        var tokenKey = "token";
        var loginData = {
            grant_type: 'password',
            username: f.value.email,
            password: f.value.pass
        };
        
        this.http.post<any>("http://localhost:60000/api/account/Token", loginData)
        .subscribe(x => {
            debugger;
            sessionStorage.setItem(tokenKey, x.access_token);

            this.http.get<any>("http://localhost:60000/api/values/GetRole", {
                    headers: {
                        Authorization: "Bearer " + sessionStorage.getItem(tokenKey)
                }})
                .subscribe(x => {
                    debugger;
                    console.log("test");
                })
        });
    }
}