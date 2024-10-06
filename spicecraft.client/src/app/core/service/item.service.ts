import {inject, Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {ResultDetailModel} from "../model/result-detail.model";
import {ItemSummaryModel} from "../model/item/item-summary-item.model";
import {HttpClient} from "@angular/common/http";
import {WebAPIService} from "./webAPI.service";
import {MenuItemModel} from "../model/item/menu-item.model";
import {ItemFilterModel} from "../model/item/item-filter.model";
import {LoggedInUserModel} from "../model/user/logged-in-user.model";
import {ItemDetailModel} from "../model/item/item-detail.model";

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  private _api = inject(WebAPIService);

  getItems(filterForm: ItemFilterModel): Observable<ResultDetailModel<MenuItemModel>> {
    return this._api.get<ResultDetailModel<MenuItemModel>>('/item', filterForm);
  }

  addOrRemoveItemFromListing(itemId: number, isAdd = true) {
    return this._api.get<ResultDetailModel<boolean>>(`/item/listing/${itemId}?isAdd=${isAdd}`);
  }

  getItemDetailById(itemId: number) {
    return this._api.get<ResultDetailModel<ItemDetailModel>>(`/item/${itemId}`);
  }
}
