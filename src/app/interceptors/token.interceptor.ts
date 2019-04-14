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
export class TokenInterceptor implements HttpInterceptor {
    private refreshTokenInProgress = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
    
    constructor(private authService: AuthService) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        
        request = request.clone({
        setHeaders: {
            Authorization: `Bearer ${this.authService.getAuthToken()}`
        }
        });

        return next.handle(request).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error && error.status === 401) {
                    if (this.refreshTokenInProgress) {

                    } else {
                        this.refreshTokenInProgress = true;
                        
                        this.refreshTokenSubject.next(null);
                    }
                }
            })
        );
    }
}