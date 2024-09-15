import {HttpClient} from "@angular/common/http";
import {WebAPIService} from "./webAPI.service";
import {MenuItemModel} from "../model/item/menu-item.model";
import {ItemFilterModel} from "../model/item/item-filter.model";
import {LoggedInUserModel} from "../model/user/logged-in-user.model";
import {inject, Injectable} from "@angular/core";
import {BehaviorSubject, Observable} from "rxjs";
import {ResultDetailModel} from "../model/result-detail.model";
import {UserService} from "./user.service";
import {CreateUpdateCartItemRequest} from "../model/cart/create-update-cart-item-request.model";
import {ActionSuccessModel} from "../interface/action-success.interface";
import {ShoppingCartModel} from "../model/cart/shopping-cart.model";

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private _api = inject(WebAPIService);
  private _userService = inject(UserService);
  private _showAddToCartDialog: boolean = false;
  public showAddToCartDialog$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this._showAddToCartDialog);

  constructor() { }

  toggleAddToCart() {
    this.showAddToCartDialog$.next(!this._showAddToCartDialog);
  }

  showCartDialog(): void {
    this._showAddToCartDialog = true;
    this.showAddToCartDialog$.next(this._showAddToCartDialog);
  }

  hideCartDialog(): void {
    this._showAddToCartDialog = false;
    this.showAddToCartDialog$.next(this._showAddToCartDialog);
  }

  addToCart(quantity: number, size: string, spiceLevel: string, itemId: number) {
    const userId = this._userService.getCurrentUserId();
    const createUpdateCartItem: CreateUpdateCartItemRequest = {
      quantity: quantity,
      description: '',
      itemId: itemId,
      userId: userId,
      priceAtAdd: 0,
      cartId: 0,
    }
    return this._api.post('/cart/cart-item', createUpdateCartItem)
  }

  getCartForCurrentUser() {
    const userId = this._userService.getCurrentUserId();
    return this._api.get<ResultDetailModel<ShoppingCartModel>>(`/cart/user/${userId}/cart`);
  }

  updateCart(cartItemId: number, action: string) {
    if (action === 'increment') {
      return this.incrementCartQuantity(cartItemId);
    }

    if (action === 'decrement') {
      return this.decrementCartQuantity(cartItemId);
    }

    if (action === 'delete') {
      return this.deleteCartItem(cartItemId);
    }

    return;
  }

  incrementCartQuantity(cartItemId: number) {
    const quantity = 1;
    return this._api.post<ResultDetailModel<string>>(`/cart/cart-item/${cartItemId}/increment?quantity=${quantity}`, {});
  }

  decrementCartQuantity(cartItemId: number) {
    const quantity = 1;
    return this._api.post<ResultDetailModel<string>>(`/cart/cart-item/${cartItemId}/decrement?quantity=${quantity}`, {});
  }

  deleteCartItem(cartItemId: number) {
    return this._api.delete<ResultDetailModel<string>>(`/cart/cart-item/${cartItemId}`);
  }
}
