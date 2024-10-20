import {inject, Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {WebAPIService} from "./webAPI.service";
import {CustomerDashboardModel} from "../model/customer/customer-dashboard.model";

@Injectable({
  providedIn: 'root'
})
export class CustomerDashboardService {

  private _api = inject(WebAPIService);

  getCustomerDashboard(userId: number): Observable<CustomerDashboardModel> {
    return this._api.get<CustomerDashboardModel>(`/dashboard/${userId}`);
  }
}
