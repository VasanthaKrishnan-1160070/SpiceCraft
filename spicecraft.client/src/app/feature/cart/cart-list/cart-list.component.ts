import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {CartItemModel} from "../../../core/model/cart/cart-item.model";
import {DxButtonModule, DxDataGridModule} from "devextreme-angular";
import {AsyncPipe, NgIf} from "@angular/common";
import {CartService} from "../../../core/service/cart.service";
import {take, takeUntil} from "rxjs/operators";
import {Subject} from "rxjs";
import {Router} from "@angular/router";
import {MessageService} from "../../../core/service/message.service";
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-cart-list',
  standalone: true,
    imports: [
        DxDataGridModule,
        DxButtonModule,
        NgIf,
        AsyncPipe,
        TitleComponent
    ],
  templateUrl: './cart-list.component.html',
  styleUrl: './cart-list.component.css'
})
export class CartListComponent implements OnInit, OnDestroy {
  cartItems: CartItemModel[] = [];
  private router = inject(Router);
  private _cartService: CartService = inject(CartService);
  private _messageService: MessageService = inject(MessageService);
  private _destroy$ = new Subject<void>();
  cartSummary: { finalPrice: string, savings: string } = { finalPrice: '0.00', savings: '0.00' };

  constructor() {}


  ngOnInit(): void {
     this.getCartItems();
    }

  // Navigate to the checkout page
  checkout() {
    // Add your checkout logic here
    console.log('Proceeding to checkout');
    this.router.navigate(['/customer-checkout']);
  }

  getCartItems() {
    this._cartService.getCartForCurrentUser().pipe(
      take(1),
      takeUntil(this._destroy$)
    )
      .subscribe(shoppingCart => {
        this.cartItems = shoppingCart?.data?.cartItems as CartItemModel[] || []
        this.cartSummary.finalPrice = shoppingCart?.data?.finalPrice?.toString() || '';
        this.cartSummary.savings = shoppingCart?.data?.savings?.toString() || '';
        console.log(shoppingCart);
      });
  }

  // Update quantity or delete item from cart
  updateQuantity(cartItemId: number, quantity: number, subAction: string) {
     this._cartService.updateCart(cartItemId, subAction)?.pipe(
       take(1),
       takeUntil(this._destroy$)
     )
      .subscribe(shoppingCart => {
        if (subAction === 'delete') {
          this._messageService.showConfirmDialog('Are you sure you wanted to delete?')
              .then( (result) => {
                if (result){
                  this.cartItems = [];
                }
              })
        }
          this.getCartItems();
       });
  }

  ngOnDestroy() {
    this._destroy$.next();
  }
}
