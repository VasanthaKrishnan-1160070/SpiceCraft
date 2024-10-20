import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {ResultDetailModel} from "../model/result-detail.model";
import {ProductInventoryModel} from "../model/inventory/product-inventory.model";
import {StockUpdateModel} from "../model/inventory/stock-update.model";
import {WebAPIService} from "./webAPI.service";
import {IngredientsModel} from "../model/inventory/ingredeients.model";
import {IngredientInventoryModel} from "../model/inventory/ingredient-inventory.model";
import {UpdateStockModel} from "../model/inventory/update-stock.model";



@Injectable({
  providedIn: 'root',
})
export class InventoryService {
  private _api = inject(WebAPIService);

  // Get all available products
  getAvailableProducts(): Observable<ResultDetailModel<ProductInventoryModel[]>> {
    return this._api.get<ResultDetailModel<ProductInventoryModel[]>>('/inventory/products');
  }

  // Get stock for a specific product by item ID
  getProductStock(itemId: number): Observable<ResultDetailModel<number>> {
    return this._api.get<ResultDetailModel<number>>(`/inventory/stock/${itemId}`);
  }

  // Update stock for a specific product by item ID
  updateProductStock(itemId: number, currentStock: number): Observable<ResultDetailModel<boolean>> {
    return this._api.put<ResultDetailModel<boolean>>(`/inventory/stock/update/${itemId}`, currentStock);
  }

  // Insert a new product into the inventory
  addProductToInventory(itemId: number, stockModel: StockUpdateModel): Observable<ResultDetailModel<boolean>> {
    return this._api.post<ResultDetailModel<boolean>>(`/inventory/product/add`, { itemId, ...stockModel });
  }

  // Decrement stock for a specific product by a quantity
  decrementProductStock(itemId: number, quantity: number): Observable<ResultDetailModel<number>> {
    return this._api.put<ResultDetailModel<number>>(`/inventory/stock/decrement/${itemId}`, quantity);
  }

  // for dashboards
  getLowStockIngredients(): Observable<ResultDetailModel<IngredientsModel[]>> {
    return this._api.get<ResultDetailModel<IngredientsModel[]>>(`/inventory/low-stock`);
  }

  getInventory(): Observable<ResultDetailModel<IngredientInventoryModel[]>> {
    return this._api.get<ResultDetailModel<IngredientInventoryModel[]>>(`/inventory/ingredient-inventory`);
  }

  updateStock(updateStockDTO: UpdateStockModel): Observable<any> {
    return this._api.post(`/inventory/update-ingredient-stock`, updateStockDTO);
  }
}
