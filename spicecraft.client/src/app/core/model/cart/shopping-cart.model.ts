import {CartItemModel} from "./cart-item.model";

export interface ShoppingCartModel {
  cartItems: CartItemModel[];
  totalPrice: number;
}
