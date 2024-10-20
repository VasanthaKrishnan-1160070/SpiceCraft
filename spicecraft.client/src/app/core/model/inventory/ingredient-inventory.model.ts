export interface IngredientInventoryModel {
  ingredientId: number;
  ingredientName: string;
  currentStock: number;
  reorderLevel: number;
  unit: string;
  itemsPerUnit: number;
}
