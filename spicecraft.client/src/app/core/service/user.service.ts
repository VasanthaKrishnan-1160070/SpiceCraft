import {inject, Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {WebAPIService} from "./webAPI.service";
import {HttpResponse} from "@angular/common/http";
import {map} from "rxjs/operators";
import {RegisterModel} from "../interface/register-user.interface";
import {ActionSuccessModel} from "../interface/action-success.interface";



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

  checkUserName(userName: string) {
    return this._api.get<{valid: boolean}>(`/user/check-username/${userName}`)
          .pipe(map(response => response.valid));
  }

  checkEmail(email: string) {
    return this._api.get<{valid: boolean}>(`/user/check-email/${email}`)
      .pipe(map(response => response.valid));
  }

  registerCustomer(customer: RegisterModel) {
    return this._api.post<ActionSuccessModel>('/user/register', customer)
  }

  registerStaff(customer: RegisterModel) {
    return this._api.post<ActionSuccessModel>('/user/register', customer)
  }
}
