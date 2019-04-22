import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { of } from 'rxjs/internal/observable/of';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable()
export class AuthService {
  private tokenKey = "token";
  private refreshTokenKey = "refreshToken"
  constructor(private http: HttpClient) {

  }
  
  public getAuthToken(): string {
    return localStorage.getItem(this.tokenKey);
  }

  public getRefreshToken(): string {
    return localStorage.getItem(this.refreshTokenKey);
  }

  public getAccessToken(username: string, password: string) : Observable<any> {
    return this.http.post<any>("/account/token", {username, password})
      .pipe(map(response => { //handle error
        this.setTokens(response.accessToken, response.refreshToken);
          return response;
      }));
  }

  public refreshAccessToken(): Observable<any> {
    return this.http.post<any>("/account/refresh", {
      accessToken: this.getAuthToken(),
      refreshToken: this.getRefreshToken()
    }).pipe(map(response => {
      this.setTokens(response.accessToken, response.refreshToken);
      return response;
    }));
  }

  private setTokens(access: string, refresh: string) : void {
    localStorage.setItem(this.tokenKey, access);
    localStorage.setItem(this.refreshTokenKey, refresh);
  }
}