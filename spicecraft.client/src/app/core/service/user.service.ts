import {inject, Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {WebAPIService} from "./webAPI.service";
import {HttpParams, HttpResponse} from "@angular/common/http";
import {map} from "rxjs/operators";
import {RegisterModel} from "../interface/register-user.interface";
import {ActionSuccessModel} from "../interface/action-success.interface";
import {UserRoleEnum} from "../enum/user-role.enum";
import {UserModel} from "../model/user/user.model";
import {LoggedInUserModel} from "../model/user/logged-in-user.model";
import {log} from "@angular-devkit/build-angular/src/builders/ssr-dev-server";
import {CreateUpdateCartItemRequest} from "../model/cart/create-update-cart-item-request.model";
import {AuthService} from "./auth.service";
import {ResultDetailModel} from "../model/result-detail.model";



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
    userName: '',
    isActive: true
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

  getUserFirstName(): string {
    const loggedInUser = this.getLoggedInUser();
    return `${loggedInUser.firstName}`;
  }

  getCurrentUserId(): number {
    const loggedInUser = this.getLoggedInUser();
    return loggedInUser.userId;
  }

  checkUserName(userName: string, userId?: number) {
    const url = userId
      ? `/user/check-username/${userName}?userId=${userId}`
      : `/user/check-username/${userName}`;

    return this._api.get<{ valid: boolean }>(url)
      .pipe(map(response => response.valid));
  }

  checkEmail(email: string, userId?: number) {
    const url = userId
      ? `/user/check-email/${email}?userId=${userId}`
      : `/user/check-email/${email}`;

    return this._api.get<{ valid: boolean }>(url)
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

  createOrUpdate(userDetail: UserModel){
    return this._api.post<ResultDetailModel<boolean>>('/user/create-or-update', userDetail)
  }

  getUserDetailsById(userId: number) {
    return this._api.get<ResultDetailModel<UserModel>>(`/user/${userId}`)
  }

  changePassword(userId: number, newPassword: string, oldPassword: string)  {
    return this._api.post(`/user/change-password/}`, {userId, newPassword, oldPassword});
  }

  toggleUserActive(userId: number){
    return this._api.post(`/user/toggle-user-active/${userId}`, {});
  }
}
