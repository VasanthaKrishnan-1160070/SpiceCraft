import {Component, inject, OnInit} from '@angular/core';
import {CartItemModel} from "../../../core/model/cart/cart-item.model";
import {DxButtonModule, DxDataGridModule} from "devextreme-angular";
import {AsyncPipe, NgIf} from "@angular/common";
import {CartService} from "../../../core/service/cart.service";
import {take, takeUntil} from "rxjs/operators";
import {Subject} from "rxjs";

@Component({
  selector: 'sc-cart-list',
  standalone: true,
  imports: [
    DxDataGridModule,
    DxButtonModule,
    NgIf,
    AsyncPipe
  ],
  templateUrl: './cart-list.component.html',
  styleUrl: './cart-list.component.css'
})
export class CartListComponent implements OnInit {
  cartItems: CartItemModel[] = [];
  private _cartService: CartService = inject(CartService);
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
  }

  getCartItems() {
    this._cartService.getCartForCurrentUser().pipe(
      take(1)
    )
      .subscribe(shoppingCart => {
        this.cartItems = shoppingCart.data?.cartItems as CartItemModel[];
        this.cartSummary.finalPrice = shoppingCart.data?.finalPrice?.toString() || '';
        this.cartSummary.savings = shoppingCart.data?.savings?.toString() || '';
        console.log(shoppingCart);
      });
  }

  // Update quantity or delete item from cart
  updateQuantity(cartItemId: number, quantity: number, subAction: string) {
     this._cartService.updateCart(cartItemId, subAction)?.pipe(
       takeUntil(this._destroy$)
     )
      .subscribe(shoppingCart => {
          this.getCartItems();
       });
  }
}
