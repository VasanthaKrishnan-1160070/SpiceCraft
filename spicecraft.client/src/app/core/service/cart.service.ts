import {HttpClient} from "@angular/common/http";
import {WebAPIService} from "./webAPI.service";
import {MenuItemModel} from "../model/item/menu-item.model";
import {ItemFilterModel} from "../model/item/item-filter.model";
import {LoggedInUserModel} from "../model/user/logged-in-user.model";
import {inject, Injectable} from "@angular/core";
import {BehaviorSubject, Observable} from "rxjs";
import {ResultDetailModel} from "../model/result-detail.model";

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private _api = inject(WebAPIService);
  private _showAddToCartDialog: boolean = false;
  public showAddToCartDialog$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this._showAddToCartDialog);

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
}
