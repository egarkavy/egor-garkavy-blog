import { Injectable } from '@angular/core';
import { tokenNotExpired } from 'angular2-jwt';
import { Observable } from 'rxjs/internal/Observable';
import { of } from 'rxjs/internal/observable/of';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable()
export class AuthService {
  private tokenKey = "token";
  constructor(private http: HttpClient) {

  }
  
  public getAuthToken(): string {
    return localStorage.getItem('token');
  }

  public getRefreshToken(): string {
    return localStorage.getItem('refreshToken');
  }

  public isAuthenticated(): boolean {
    const token = this.getAuthToken();
    
    return tokenNotExpired(null, token);
  }

  public getAccessToken(username: string, password: string) : Observable<any> {
    return this.http.post<any>("account/token", {username, password})
      .pipe(map(response => { //handle error
          sessionStorage.setItem(this.tokenKey, response.access_token);
      }));
  }

  public refreshAccessToken(): Observable<any> {
    return this.http.post("/account/refresh", {
      token: this.getAuthToken(),
      refreshToken: this.getRefreshToken()
    })
  }
}