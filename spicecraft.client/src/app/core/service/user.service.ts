import {inject, Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {WebAPIService} from "./webAPI.service";
import {HttpResponse} from "@angular/common/http";
import {map} from "rxjs/operators";
import {RegisterModel} from "../interface/register-user.interface";
import {ActionSuccessModel} from "../interface/action-success.interface";
import {UserRoleEnum} from "../enum/user-role.enum";
import {UserModel} from "../model/user/user.model";
import {LoggedInUserModel} from "../model/user/logged-in-user.model";
import {log} from "@angular-devkit/build-angular/src/builders/ssr-dev-server";
import {CreateUpdateCartItemRequest} from "../model/cart/create-update-cart-item-request.model";
import {AuthService} from "./auth.service";



@Injectable({
  providedIn: 'root'
})
export class UserService {

  private _api = inject(WebAPIService);

  loggedInUser: LoggedInUserModel = {
    userId: 0,
    firstName: '',
    lastName: '',
    email: '',
    roleName: '',
    userName: ''
  }

  constructor() { }

  getLoggedInUser(): LoggedInUserModel {
    let loggedInUser = localStorage.getItem('loggedInUser');
    if (loggedInUser) {
      return JSON.parse(loggedInUser) as LoggedInUserModel;
    }
    return this.loggedInUser;
  }

  getCurrentUserName(): string {
    const loggedInUser = this.getLoggedInUser();
    return `${loggedInUser.firstName} ${loggedInUser.lastName}`;
  }

  getCurrentUserId(): number {
    const loggedInUser = this.getLoggedInUser();
    return loggedInUser.userId;
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

  getStaffs() {
    return this._api.get<UserModel[]>(`/user/user-role/${UserRoleEnum.Staff}`);
  }

  getCustomers() {
    return this._api.get<UserModel[]>(`/user/user-role/${UserRoleEnum.Customer}`);
  }

  getDealers() {
    return this._api.get<UserModel[]>(`/user/user-role/${UserRoleEnum.Manager}`);
  }
}
