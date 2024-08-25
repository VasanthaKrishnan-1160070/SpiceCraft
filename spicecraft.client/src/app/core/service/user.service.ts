import {inject, Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {WebAPIService} from "./webAPI.service";
import {HttpResponse} from "@angular/common/http";



@Injectable({
  providedIn: 'root'
})
export class UserService {

  private _api = inject(WebAPIService)

  loggedInUser = {
    firstName: '',
    email: '',
    roleName: '',
    userName: ''
  }

  constructor() { }

  getLoggedInUser() {
    let loggedInUser = localStorage.getItem('loggedInUser');
    if (loggedInUser) {
      return JSON.parse(loggedInUser);
    }
  }


}
