import {Component, inject} from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {DxButtonModule, DxDataGridModule, DxTemplateModule} from "devextreme-angular";
import {
  DxiColumnModule,
  DxoColumnChooserModule,
  DxoLoadPanelModule,
  DxoPagerModule, DxoPagingModule
} from "devextreme-angular/ui/nested";
import {CartItemModel} from "../../../core/model/cart/cart-item.model";
import {Router, RouterLink} from "@angular/router";
import {CartService} from "../../../core/service/cart.service";
import {Observable, Subject} from "rxjs";
import {take, takeUntil} from "rxjs/operators";
import {OrderService} from "../../../core/service/order.service";
import {UserService} from "../../../core/service/user.service";
import {MessageService} from "../../../core/service/message.service";
import {UserOrderModel} from "../../../core/model/order/user-order.model";
import {AuthService} from "../../../core/service/auth.service";
import {ResultDetailModel} from "../../../core/model/result-detail.model";
import {NgIf} from "@angular/common";

@Component({
  selector: 'sc-order-list',
  standalone: true,
  imports: [
    TitleComponent,
    DxButtonModule,
    DxDataGridModule,
    DxTemplateModule,
    DxiColumnModule,
    DxoColumnChooserModule,
    DxoLoadPanelModule,
    DxoPagerModule,
    DxoPagingModule,
    RouterLink,
    NgIf
  ],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css'
})
export class OrderListComponent {

  public isInternaluser = false;
  private router = inject(Router);
  private _orderService: OrderService = inject(OrderService);
  private _userService: UserService = inject(UserService);
  private _messageService: MessageService = inject(MessageService);
  private _authService = inject(AuthService);
  private _destroy$ = new Subject<void>();

  public orderItems!: UserOrderModel[];


  ngOnInit(): void {
    this.isInternaluser = this._authService.isInternalUser();
    this.getOrderItems();
  }

  getOrderItems() {
    let orderItems$!: Observable<ResultDetailModel<UserOrderModel[]>>;

    if (this._authService.isInternalUser()) {
      orderItems$ = this._orderService.getAllOrders().pipe(
        take(1),
        takeUntil(this._destroy$)
      );
    }
    else {
      orderItems$ = this._orderService.getUserOrders(this._userService.getCurrentUserId()).pipe(
        take(1),
        takeUntil(this._destroy$)
      );
    }

    orderItems$.subscribe(orderItems => {
      this.orderItems = orderItems?.data || [];
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
