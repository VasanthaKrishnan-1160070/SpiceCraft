import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {RecentlyViewedItemModel} from "../model/recentlyViewed/RecentlyViewedItemModel";
import {ResultDetailModel} from "../model/result-detail.model";


@Injectable({
  providedIn: 'root'
})
export class RecentlyViewedItemService {
  private apiUrl = '/api/RecentlyViewedItems';

  constructor(private http: HttpClient) {}

  getRecentlyViewedItems(userId: number): Observable<ResultDetailModel<RecentlyViewedItemModel[]>> {
    return this.http.get<ResultDetailModel<RecentlyViewedItemModel[]>>(`${this.apiUrl}/user/${userId}`);
  }

  addRecentlyViewedItem(item: RecentlyViewedItemModel): Observable<any> {
    return this.http.post(`${this.apiUrl}/add`, item);
  }
}
