import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { AuthService } from './../services/auth.service';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, filter, finalize, switchMap, take } from 'rxjs/operators'

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    private refreshTokenInProgress = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
    
    constructor(private authService: AuthService) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        
        request = this.addAuthenticationToken(request);

        return next.handle(request).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error && error.status === 401) {
                    if (this.refreshTokenInProgress) {
                        return this.refreshTokenSubject.pipe(
                            filter(result => result !== null),
                            take(1),
                            switchMap(() => next.handle(this.addAuthenticationToken(request)))
                          );
                    } else {
                        this.refreshTokenInProgress = true;

                        this.refreshTokenSubject.next(null);

                        return this.authService.refreshAccessToken().pipe(
                            switchMap((success: boolean) => {               
                              this.refreshTokenSubject.next(success);
                              return next.handle(this.addAuthenticationToken(request));
                            }),
                            finalize(() => this.refreshTokenInProgress = false)
                          );
                    }
                } else {
                    return throwError(error);
                }
            })
        );
    }

    private addAuthenticationToken(request: HttpRequest<any>): HttpRequest<any> {
        const token = this.authService.getAuthToken();

        if (!token) {
            return request;
        }
        
        request = request.clone({
            setHeaders: {
                Authorization: `Bearer ${this.authService.getAuthToken()}`
            }});

        return request;
      }
}