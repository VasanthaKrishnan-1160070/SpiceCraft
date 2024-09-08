import {inject, Injectable} from '@angular/core';
import {WebAPIService} from "./webAPI.service";
import {CategoryModel} from "../model/category/category.model";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private _api = inject(WebAPIService);

  public getCategories(): Observable<CategoryModel[]> {
    return this._api.get<CategoryModel[]>('/category');
  }

  public getSubCategories(categoryId: number) : Observable<CategoryModel[]> {
    return this._api.get<CategoryModel[]>(`/category/${categoryId}/sub-categories`);
  }
}
