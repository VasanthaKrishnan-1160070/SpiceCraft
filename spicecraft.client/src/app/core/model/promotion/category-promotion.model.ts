export interface CategoryPromotionModel {
  categoryId: number;
  categoryName: string;
  parentCategoryName: string;
  discountRate: string;
  hasPromotion: string;
  actionHidden: string;
  actionAdd: boolean;
  actionRemove: boolean;
}
