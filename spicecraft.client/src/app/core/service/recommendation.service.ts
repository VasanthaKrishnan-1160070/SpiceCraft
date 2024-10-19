import {inject, Injectable} from "@angular/core";
import {WebAPIService} from "./webAPI.service";
import {Observable} from "rxjs";
import {ResultDetailModel} from "../model/result-detail.model";
import {MenuItemModel} from "../model/item/menu-item.model";
import {RecentlyViewedItemModel} from "../model/recentlyViewed/RecentlyViewedItemModel";

@Injectable({
  providedIn: 'root'
})
export class RecommendationService {
  private _api = inject(WebAPIService);

  trainRecommendationModel(userId: number): Observable<ResultDetailModel<MenuItemModel[]>> {
    return this._api.post<ResultDetailModel<MenuItemModel[]>>(`/recommendation/train`, {});
  }

  getRecommendedItems(userId: number, noOfRecords: number = 10): Observable<MenuItemModel[]> {
    return this._api.get<MenuItemModel[]>(`/recommendation/user/${userId}/top/${noOfRecords}`);
  }
}
