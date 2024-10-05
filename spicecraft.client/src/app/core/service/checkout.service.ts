import {inject, Injectable} from '@angular/core';
import { Observable } from 'rxjs';
import {WebAPIService} from "./webAPI.service";
import {ResultDetailModel} from "../model/result-detail.model";
import {CheckoutDetailModel} from "../model/checkout/checkout-detail.model";
import {HttpParams} from "@angular/common/http";
import {StartPostPaymentModel} from "../model/checkout/start-post-payment.model";
import {GiftCardModel} from "../model/checkout/gift-card.model";

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  private _api = inject(WebAPIService);


  // Get checkout details for a user
  getCheckoutInfo(userId: number): Observable<ResultDetailModel<CheckoutDetailModel>> {
    return this._api.get<ResultDetailModel<CheckoutDetailModel>>(`/checkout/${userId}`);
  }

  // Get total amount to pay for a user, including shipping option if provided
  getTotalAmount(userId: number, shippingOptionId?: number): Observable<ResultDetailModel<number>> {
    let params = new HttpParams();
    if (shippingOptionId !== undefined && shippingOptionId !== null) {
      params = params.append('shippingOptionId', shippingOptionId.toString());
    }

    return this._api.get<ResultDetailModel<number>>(`/checkout/totalamount/${userId}`, { params });
  }

  // Apply a gift card for a user
  applyGiftCard(code: string, userId: number): Observable<ResultDetailModel<GiftCardModel>> {
    const request = { code, userId };
    return this._api.post<ResultDetailModel<GiftCardModel>>(`/checkout/applygiftcard`, request);
  }

  // Start the post-payment process, including order creation and payment
  startPostPaymentProcess(model: StartPostPaymentModel): Observable<ResultDetailModel<boolean>> {
    return this._api.post<ResultDetailModel<boolean>>(`/checkout/startpostpayment`, model);
  }
}
