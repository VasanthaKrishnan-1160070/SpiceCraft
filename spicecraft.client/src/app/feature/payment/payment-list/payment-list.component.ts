import {Component, inject, OnInit} from '@angular/core';
import {PaymentDetailComponent} from "../payment-detail/payment-detail.component";
import {Router, RouterLink} from "@angular/router";
import {PaymentService} from "../../../core/service/payment.service";
import {UserService} from "../../../core/service/user.service";
import {MessageService} from "../../../core/service/message.service";
import {Subject} from "rxjs";
import {PaymentModel} from "../../../core/model/payment/payment.model";
import {take, takeUntil} from "rxjs/operators";
import {DxButtonModule, DxDataGridModule, DxTemplateModule} from "devextreme-angular";
import {
  DxiColumnModule,
  DxoColumnChooserModule,
  DxoHeaderFilterModule,
  DxoLoadPanelModule, DxoPagerModule, DxoPagingModule, DxoSearchPanelModule
} from "devextreme-angular/ui/nested";
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-payment-list',
  standalone: true,
  imports: [
    PaymentDetailComponent,
    DxButtonModule,
    DxDataGridModule,
    DxTemplateModule,
    DxiColumnModule,
    DxoColumnChooserModule,
    DxoHeaderFilterModule,
    DxoLoadPanelModule,
    DxoPagerModule,
    DxoPagingModule,
    DxoSearchPanelModule,
    TitleComponent,
    RouterLink
  ],
  templateUrl: './payment-list.component.html',
  styleUrl: './payment-list.component.css'
})
export class PaymentListComponent implements  OnInit {

private router = inject(Router);
private _paymentService: PaymentService = inject(PaymentService);
private _userService: UserService = inject(UserService);
private _messageService: MessageService = inject(MessageService);
private _destroy$ = new Subject<void>();

public paymentItems!: PaymentModel[];


  ngOnInit(): void {
    this.getPaymentItems();
  }

  getPaymentItems() {
    this._paymentService.getPaymentsForUser(this._userService.getCurrentUserId()).pipe(
      take(1),
      takeUntil(this._destroy$)
    )
      .subscribe(paymentItems => {
        this.paymentItems = paymentItems?.data || [];
        console.log(this.paymentItems);
      });
  }

// Update quantity or delete item from cart
  cancelOrder(orderId: number) {
    // this._orderService.updateCart(cartItemId, subAction)?.pipe(
    //   take(1),
    //   takeUntil(this._destroy$)
    // )
    //   .subscribe(shoppingCart => {
    //     if (subAction === 'delete') {
    //       this.cartItems = [];
    //     }
    //     this.getCartItems();
    //   });
  }

  ngOnDestroy() {
    this._destroy$.next();
  }
}
