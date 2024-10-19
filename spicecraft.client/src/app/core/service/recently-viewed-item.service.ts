import {inject, Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {RecentlyViewedItemModel} from "../model/recentlyViewed/RecentlyViewedItemModel";
import {ResultDetailModel} from "../model/result-detail.model";
import {MenuItemModel} from "../model/item/menu-item.model";
import {WebAPIService} from "./webAPI.service";


@Injectable({
  providedIn: 'root'
})
export class RecentlyViewedItemService {
  private apiUrl = '/api/RecentlyViewedItems';
  private _api = inject(WebAPIService);



  getRecentlyViewedItems(userId: number): Observable<ResultDetailModel<MenuItemModel[]>> {
    return this._api.get<ResultDetailModel<MenuItemModel[]>>(`/recentlyViewedItems/user/${userId}`);
  }

  addRecentlyViewedItem(item: RecentlyViewedItemModel): Observable<any> {
    return this._api.post(`/recentlyViewedItems/add`, item);
  }
}
