
export interface ItemSummaryModel {
  itemId: number;
  itemName: string;
  description: string;
  createdAt?: Date | null;          // Nullable DateTime
  price: number;
  ownProduct?: boolean | null;      // Nullable boolean
  isRemoved?: boolean | null;       // Nullable boolean
  categoryName: string;
  parentCategoryId?: number | null; // Nullable integer
  subCategoryId: number;
  discountRate?: number | null;     // Nullable decimal
  currentStock?: number | null;     // Nullable integer
  isMain?: boolean | null;          // Nullable boolean
}
