import {inject, Injectable} from '@angular/core';
import {WebAPIService} from "./webAPI.service";
import {Observable, of} from "rxjs";
import { tap, map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  _api = inject(WebAPIService);
  constructor() { }

  login(username: string, password: string): Observable<boolean> {
    return this._api.post<any>(`/authentication/login`, { username, password }).pipe(
      tap((response) => {
        const token = response?.token;
        if (token) {
          localStorage.setItem('authToken', token);
        }
      }),
      map((response) => !!response?.token), // Return true if the token exists, otherwise false
      catchError((error) => {
        console.error('Login failed', error);
        return of(false); // Return false in case of error
      })
    );
  }


  logout() {
    localStorage.removeItem('authToken');  // Remove the token on logout
    // Redirect to login page or another page
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('authToken');  // Check if token exists
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }
}
