export interface BogoPromotionModel {
  itemId: number;
  itemName: string;
  categoryName: string;
  actualPrice: string; // formatted as currency
  comboName: string;
  buyQuantity: string;
  getQuantity: string;
  hasPromotion?: string;
  actionHidden?: string;
  actionAdd?: boolean;
  actionRemove?: boolean;
}
