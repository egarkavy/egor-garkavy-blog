import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { AuthService } from './../services/auth.service';
import { Observable, BehaviorSubject } from 'rxjs';
import { catchError } from 'rxjs/operators'

@Injectable()
export class UrlInterceptor implements HttpInterceptor {
    
    constructor() {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const urlPrefix = "http://localhost:60000/api";

        request = request.clone({
            url:  urlPrefix.concat(request.url)
        });

        return next.handle(request);
    }
}