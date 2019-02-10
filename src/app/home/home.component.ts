import { Component } from "@angular/core";
import { HttpClient } from '@angular/common/http';
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

        var tokenKey = "accessToken";
        var loginData = {
            grant_type: 'password',
            username: f.value.email,
            password: f.value.pass
        };
        
        this.http.post<object>("http://localhost:60000/api/account/Token", loginData)
        .subscribe(x => {
            debugger;
            sessionStorage.setItem(tokenKey, "asd");
        });
    }
}