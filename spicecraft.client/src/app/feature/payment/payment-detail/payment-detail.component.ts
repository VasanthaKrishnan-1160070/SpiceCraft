import {Component, inject, OnInit} from '@angular/core';
import {DxDataGridModule, DxFormModule} from "devextreme-angular";
import {TitleComponent} from "../../../shared/components/title/title.component";
import {PaymentInvoiceModel} from "../../../core/model/payment/payment-invoice.model";
import {PaymentService} from "../../../core/service/payment.service";
import {Subject} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import {ContactInfoModel} from "../../../core/model/user/contact-info.model";
import {UserAddressModel} from "../../../core/model/user/user-address.model";
import {OrderDetailModel} from "../../../core/model/order/order-detail-model";
import {CurrencyPipe} from "@angular/common";

@Component({
  selector: 'sc-payment-detail',
  standalone: true,
  imports: [
    DxDataGridModule,
    DxFormModule,
    TitleComponent,
    CurrencyPipe
  ],
  templateUrl: './payment-detail.component.html',
  styleUrl: './payment-detail.component.css'
})
export class PaymentDetailComponent implements OnInit {
  paymentInfo?: PaymentInvoiceModel | undefined | null;
  orderPayments: any = {};  // Order payment details
  contactInfo!: ContactInfoModel | null | undefined;    // Contact information
  userAddress!: UserAddressModel | null | undefined;    // Shipping address
  orderDetails: OrderDetailModel[] = []; // List of order items


  private _paymentService: PaymentService = inject(PaymentService);
  private _destroy$: Subject<void> = new Subject<void>();
  private _router: ActivatedRoute = inject(ActivatedRoute);

  ngOnInit(): void {
    this._router.paramMap.subscribe(params => {
      const transactionId = +(params.get('transactionId') || 0);
      this.loadPaymentDetails(transactionId);
    });
  }

  loadPaymentDetails(transactionId: number): void {
    //  Load data for orderPayments, contactInfo, userAddress, orderDetails
    this._paymentService.getPaymentInvoiceDetails(transactionId)
      .subscribe(response => {
        this.paymentInfo = response.data;
        this.contactInfo = response.data?.contactInfo;
        this.userAddress = response.data?.userAddress;
        this.orderDetails = response.data?.orderDetails as OrderDetailModel[];
      });
  }
}
