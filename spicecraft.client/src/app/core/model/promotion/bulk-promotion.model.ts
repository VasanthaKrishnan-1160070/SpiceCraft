export interface BulkPromotionModel {
  itemId: number;
  itemName: string;
  categoryName: string;
  actualPrice: string; // formatted as currency
  requiredQuantityForPromotion: string;
  discountRate: string; // formatted as percentage
  hasPromotion?: string;
  actionHidden?: string;
  actionAdd?: boolean;
  actionRemove?: boolean;
}
