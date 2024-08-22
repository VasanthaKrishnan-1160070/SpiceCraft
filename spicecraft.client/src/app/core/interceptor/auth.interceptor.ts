import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('authToken');  // Get the token from localStorage

    if (token) {
      // Clone the request to add the new header
      const clonedRequest = request.clone({
        headers: request.headers.set('Authorization', `Bearer ${token}`)
      });

      // Pass the cloned request instead of the original request to the next handle
      return next.handle(clonedRequest);
    }

    // Pass the original request to the next handle if no token
    return next.handle(request);
  }
}
