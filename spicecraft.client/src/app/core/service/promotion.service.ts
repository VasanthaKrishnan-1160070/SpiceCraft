import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {WebAPIService} from "./webAPI.service";
import {ResultDetailModel} from "../model/result-detail.model";
import {PromotionModel} from "../model/promotion/promotion.model";
import {ItemPromotionModel} from "../model/promotion/item-promotion.model";
import {CategoryPromotionModel} from "../model/promotion/category-promotion.model";
import {BogoPromotionModel} from "../model/promotion/bogo-promotion.model";
import {BulkPromotionModel} from "../model/promotion/bulk-promotion.model";

@Injectable({
  providedIn: 'root',
})
export class PromotionService {
  private _api = inject(WebAPIService);

  // Get all promotions (items, categories, BOGO, bulk)
  getPromotions(): Observable<ResultDetailModel<PromotionModel>> {
    return this._api.get<ResultDetailModel<PromotionModel>>('/promotion/get-promotions');
  }
// Add a new item promotion
  addItemPromotion(promotion: ItemPromotionModel): Observable<ResultDetailModel<boolean>> {
    return this._api.post<ResultDetailModel<boolean>>('/promotion/add-item-promotion', promotion);
  }

  // Add a new category promotion
  addCategoryPromotion(promotion: CategoryPromotionModel): Observable<ResultDetailModel<boolean>> {
    return this._api.post<ResultDetailModel<boolean>>('/promotion/add-category-promotion', promotion);
  }

  // Add a new BOGO promotion
  addBogoPromotion(promotion: BogoPromotionModel): Observable<ResultDetailModel<boolean>> {
    return this._api.post<ResultDetailModel<boolean>>('/promotion/add-bogo-promotion', promotion);
  }

  // Add a new bulk item promotion
  addBulkItemPromotion(promotion: BulkPromotionModel): Observable<ResultDetailModel<boolean>> {
    return this._api.post<ResultDetailModel<boolean>>('/promotion/add-bulk-item-promotion', promotion);
  }

  // Remove an item promotion by item ID
  removeItemPromotion(itemId: number): Observable<ResultDetailModel<boolean>> {
    return this._api.delete<ResultDetailModel<boolean>>(`/promotion/remove-item-promotion/${itemId}`);
  }

  // Remove a category promotion by category ID
  removeCategoryPromotion(categoryId: number): Observable<ResultDetailModel<boolean>> {
    return this._api.delete<ResultDetailModel<boolean>>(`/promotion/remove-category-promotion/${categoryId}`);
  }

  // Remove a BOGO promotion by item ID
  removeBogoPromotion(itemId: number): Observable<ResultDetailModel<boolean>> {
    return this._api.delete<ResultDetailModel<boolean>>(`/promotion/remove-bogo-promotion/${itemId}`);
  }

  // Remove a bulk item promotion by item ID
  removeBulkItemPromotion(itemId: number): Observable<ResultDetailModel<boolean>> {
    return this._api.delete<ResultDetailModel<boolean>>(`/promotion/remove-bulk-item-promotion/${itemId}`);
  }

  // Remove all item promotions
  removeAllItemPromotions(): Observable<ResultDetailModel<boolean>> {
    return this._api.delete<ResultDetailModel<boolean>>('/promotion/remove-all-item-promotions');
  }

  // Remove all category promotions
  removeAllCategoryPromotions(): Observable<ResultDetailModel<boolean>> {
    return this._api.delete<ResultDetailModel<boolean>>('/promotion/remove-all-category-promotions');
  }

  // Remove all BOGO promotions
  removeAllBogoPromotions(): Observable<ResultDetailModel<boolean>> {
    return this._api.delete<ResultDetailModel<boolean>>('/promotion/remove-all-bogo-promotions');
  }

  // Remove all bulk item promotions
  removeAllBulkItemPromotions(): Observable<ResultDetailModel<boolean>> {
    return this._api.delete<ResultDetailModel<boolean>>('/promotion/remove-all-bulk-item-promotions');
  }
}
