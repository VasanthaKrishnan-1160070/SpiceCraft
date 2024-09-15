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
  totalPrice: { totalPrice: string, totalSavings: string } = { totalPrice: '0.00', totalSavings: '0.00' };

  constructor() {
    // Sample data, replace with real data fetching logic
    // this.cartItems = [
    //   { itemName: 'Product 1', quantity: 1, actualPrice: '50.00', finalPrice: '45.00', cartItemId: 1, cartId: 1, itemId: 1, priceAtAdd: 45.00 },
    //   { itemName: 'Product 2', quantity: 2, actualPrice: '100.00', finalPrice: '90.00', cartItemId: 2, cartId: 1, itemId: 2, priceAtAdd: 90.00 }
    // ];

    // Sample total price and savings
    this.totalPrice = { totalPrice: '135.00', totalSavings: '15.00' };
  }

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
