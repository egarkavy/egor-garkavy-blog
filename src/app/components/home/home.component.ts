import { Component } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs";
import { map } from 'rxjs/operators';
import { NgForm } from '@angular/forms';
import { AuthService } from "../../services/auth.service";

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent{ 
    constructor(private http: HttpClient, private authService : AuthService) {
       
    }

    onSubmit(f: NgForm) {
        debugger;

        var tokenKey = "token";
        var loginData = {
            grant_type: 'password',
            username: f.value.email,
            password: f.value.pass
        };
        
        this.authService.getAccessToken(loginData.username, loginData.password)
        .subscribe(response => {
            debugger;
            console.log(response);
        });
    }

    onProtectedCall() {
        this.http.get<any>("/values/GetRole")
        .subscribe(response => {
            debugger;
            console.log(response);
        })
    }
}