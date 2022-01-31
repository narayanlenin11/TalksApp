import { Injectable } from '@angular/core'
import { HttpInterceptor, HttpEvent, HttpRequest, HttpHandler } from '@angular/common/http'
import { Observable } from 'rxjs'

@Injectable()
export class TokenInterceptorService implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const jwtToken = localStorage.getItem("token");
    if (jwtToken) {
      const requestWithJwtToken = req.clone({
        headers: req.headers.set("Authorization", "Bearer " + jwtToken)
      });
      return next.handle(requestWithJwtToken);
    }
    else {
      return next.handle(req);
    }
  }
}
