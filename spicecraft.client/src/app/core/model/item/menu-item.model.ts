export interface MenuItemModel {
  price: number;
  productName: string;
  description: string;
  createdDate?: Date | null;    // Nullable Date
  isRemoved?: boolean | null;   // Nullable boolean
  currentStock: number;
  ownProduct?: boolean | null;  // Nullable boolean
  categoryId: number;
  imageCode: string;
  categoryName: string;
  productId: number;
  discountRate?: number | null; // Nullable decimal
  bulkDiscountRate?: number | null; // Nullable decimal
  bulkDiscountRequiredQuantity: number;
  comboName?: string | null;    // Nullable string
  discountPrice?: number | null; // Nullable decimal
  isInSale: string;
  isLowStock: string;
  isNoStock: string;
}
