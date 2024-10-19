import {IngredientsModel} from "./ingredeients.model";

export interface ProductInventoryModel {
  itemId: number;
  itemName: string;
  categoryName: string;
  productPrice: string;  // Formatted as '$xx.xx'
  availableStock: number;
  minimumRequiredStock: number;
  ingredients: IngredientsModel[];
}
