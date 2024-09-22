import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { WebAPIService } from './webAPI.service';
import {ResultDetailModel} from "../model/result-detail.model";
import {PaymentModel} from "../model/payment/payment.model";
import {PaymentInvoiceModel} from "../model/payment/payment-invoice.model";


@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private _api = inject(WebAPIService);

  // Get payments for a specific user
  getPaymentsForUser(userId: number): Observable<ResultDetailModel<PaymentModel[]>> {
    return this._api.get<ResultDetailModel<PaymentModel[]>>(`payment/user/${userId}`);
  }

  // Get payments for internal users
  getPaymentsForInternalUsers(): Observable<ResultDetailModel<PaymentModel[]>> {
    return this._api.get<ResultDetailModel<PaymentModel[]>>(`payment/internal`);
  }

  // Get detailed invoice information for a specific transaction
  getPaymentInvoiceDetails(transactionId: number): Observable<ResultDetailModel<PaymentInvoiceModel>> {
    return this._api.get<ResultDetailModel<PaymentInvoiceModel>>(`payment/invoice/${transactionId}`);
  }
}
