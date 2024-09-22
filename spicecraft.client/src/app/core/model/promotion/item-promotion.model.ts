export interface ItemPromotionModel {
  itemId: number;
  itemName: string;
  categoryName: string;
  actualPrice: string; // formatted as currency
  discountRate: string; // formatted as percentage
  priceAfterDiscount: string; // formatted as currency
  hasPromotion: string;
  actionHidden: string;
  actionAdd: boolean;
  actionRemove: boolean;
}
