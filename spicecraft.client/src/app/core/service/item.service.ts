import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {ResultDetailModel} from "../model/result-detail.model";
import {ItemSummaryModel} from "../model/item/item-summary-item.model";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ItemService {

  constructor(private _http: HttpClient) { }

  getItems(): Observable<ResultDetailModel<ItemSummaryModel[]>> {
    return this._http.get<ResultDetailModel<ItemSummaryModel[]>>('/items');
  }
}
