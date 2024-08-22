import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ConstantsService } from './constants.service';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class WebAPIService {
  private _constants = inject(ConstantsService);
  private _httpClient: HttpClient = inject(HttpClient);

  constructor(private http: HttpClient) { }

  private requestPath(path: string): string {
    return path ? `${this._constants.apiUrl}${path}` : this._constants.apiUrl;
  }

  // GET request that returns HttpResponse
  public getRequest<T>(path: string, queryParams?: any): Observable<HttpResponse<T>> {
    const requestPath = this.requestPath(path);
    return this._httpClient.get<T>(requestPath, {
      params: queryParams,
      observe: 'response'
    });
  }

  // GET request that returns only the body
  public get<T>(path: string, queryParams?: any): Observable<T> {
    return this.getRequest<T>(path, queryParams).pipe(
      map((response: HttpResponse<T>) => response.body as T)
    );
  }

  // POST request that returns HttpResponse
  public postRequest<T>(path: string, body: any, headers?: HttpHeaders): Observable<HttpResponse<T>> {
    const requestPath = this.requestPath(path);
    return this._httpClient.post<T>(requestPath, body, {
      headers: headers,
      observe: 'response'
    });
  }

  // POST request that returns only the body
  public post<T>(path: string, body: any, headers?: HttpHeaders): Observable<T> {
    return this.postRequest<T>(path, body, headers).pipe(
      map((response: HttpResponse<T>) => response.body as T)
    );
  }

  // PUT request that returns HttpResponse
  public putRequest<T>(path: string, body: any, headers?: HttpHeaders): Observable<HttpResponse<T>> {
    const requestPath = this.requestPath(path);
    return this._httpClient.put<T>(requestPath, body, {
      headers: headers,
      observe: 'response'
    });
  }

  // PUT request that returns only the body
  public put<T>(path: string, body: any, headers?: HttpHeaders): Observable<T> {
    return this.putRequest<T>(path, body, headers).pipe(
      map((response: HttpResponse<T>) => response.body as T)
    );
  }

  // DELETE request that returns HttpResponse
  public deleteRequest<T>(path: string, queryParams?: any): Observable<HttpResponse<T>> {
    const requestPath = this.requestPath(path);
    return this._httpClient.delete<T>(requestPath, {
      params: queryParams,
      observe: 'response'
    });
  }

  // DELETE request that returns only the body
  public delete<T>(path: string, queryParams?: any): Observable<T> {
    return this.deleteRequest<T>(path, queryParams).pipe(
      map((response: HttpResponse<T>) => response.body as T)
    );
  }
}
