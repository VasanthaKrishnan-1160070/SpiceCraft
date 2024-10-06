import {inject, Injectable} from "@angular/core";
import {WebAPIService} from "./webAPI.service";
import {UserService} from "./user.service";
import {Observable} from "rxjs";
import {ResultDetailModel} from "../model/result-detail.model";
import {UserOrderModel} from "../model/order/user-order.model";
import {OrderDetailModel} from "../model/order/order-detail-model";
import {ChangeOrderStatusModel} from "../model/order/update-order-status.model";
import {UserOrderDetailModel} from "../model/order/user-order-detail.model";

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private _api = inject(WebAPIService);
  private _userService = inject(UserService);

  // Place an order for a user
  placeOrder(userId: number, orderId: number): Observable<ResultDetailModel<boolean>> {
    return this._api.post<ResultDetailModel<boolean>>(`/order/place-order/${userId}/${orderId}`, {});
  }

  // Place a corporate order
  placeCorporateOrder(userId: number, orderId: number): Observable<ResultDetailModel<boolean>> {
    return this._api.post<ResultDetailModel<boolean>>(`/order/place-corporate-order/${userId}/${orderId}`, {});
  }

  // Get order details
  getOrderDetails(orderId: number): Observable<ResultDetailModel<UserOrderDetailModel[]>> {
    return this._api.get<ResultDetailModel<UserOrderDetailModel[]>>(`/order/order-details/${orderId}`);
  }

  // Get all orders for a user
  getUserOrders(userId: number): Observable<ResultDetailModel<UserOrderModel[]>> {
    return this._api.get<ResultDetailModel<UserOrderModel[]>>(`/order/user-orders/${userId}`);
  }

  // Change order status
  changeOrderStatus(orderId: number, request: ChangeOrderStatusModel): Observable<ResultDetailModel<boolean>> {
    return this._api.put<ResultDetailModel<boolean>>(`/order/change-order-status/${orderId}`, request);
  }

  // Verify stock before placing an order
  verifyStockBeforeOrder(userId: number): Observable<ResultDetailModel<boolean>> {
    return this._api.get<ResultDetailModel<boolean>>(`/order/verify-stock/${userId}`);
  }
}