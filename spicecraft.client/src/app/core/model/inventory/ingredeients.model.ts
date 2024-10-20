export interface IngredientsModel {
  ingredientId: number;
  ingredientName: string;
  currentStock: number;
  reorderLevel: number;
  unit: string;
  itemsPerUnit: number;
  numberOfUnits: number;
  quantityNeeded: number;
}
