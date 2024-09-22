export interface MenuItemModel {
  price: number;
  itemName: string;
  description: string;
  createdDate?: Date | null;    // Nullable Date
  isRemoved: boolean;   // Nullable boolean
  currentStock: number;
  ownProduct?: boolean;  // Nullable boolean
  categoryId: number;
  imageCode: string;
  categoryName: string;
  itemId: number;
  discountRate?: number | null; // Nullable decimal
  bulkDiscountRate?: number | null; // Nullable decimal
  bulkDiscountRequiredQuantity: number;
  comboName?: string | null;    // Nullable string
  discountPrice?: number | null; // Nullable decimal
  isInSale: string;
  isLowStock: string;
  isNoStock: string;
}
