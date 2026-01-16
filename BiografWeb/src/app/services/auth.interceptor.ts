import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const current = this.authService.currentUser;
    if (!current || !current.token) {
      return next.handle(req);
    }

    const authReq = req.clone({
      setHeaders: { Authorization: 'Bearer ' + current.token }
    });

    return next.handle(authReq);
  }
}
