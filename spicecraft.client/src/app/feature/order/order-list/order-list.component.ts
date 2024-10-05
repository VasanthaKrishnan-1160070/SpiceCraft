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
import {Subject} from "rxjs";
import {take, takeUntil} from "rxjs/operators";
import {OrderService} from "../../../core/service/order.service";
import {UserService} from "../../../core/service/user.service";
import {MessageService} from "../../../core/service/message.service";
import {UserOrderModel} from "../../../core/model/order/user-order.model";

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
    RouterLink
  ],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css'
})
export class OrderListComponent {
  private router = inject(Router);
  private _orderService: OrderService = inject(OrderService);
  private _userService: UserService = inject(UserService);
  private _messageService: MessageService = inject(MessageService);
  private _destroy$ = new Subject<void>();

  public orderItems!: UserOrderModel[];


  ngOnInit(): void {
    this.getOrderItems();
  }

  getOrderItems() {
    this._orderService.getUserOrders(this._userService.getCurrentUserId()).pipe(
      take(1),
      takeUntil(this._destroy$)
    )
      .subscribe(orderItems => {
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
