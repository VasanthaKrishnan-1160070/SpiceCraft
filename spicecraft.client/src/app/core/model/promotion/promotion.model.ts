import {BulkPromotionModel} from "./bulk-promotion.model";
import {BogoPromotionModel} from "./bogo-promotion.model";
import {CategoryPromotionModel} from "./category-promotion.model";
import {ItemPromotionModel} from "./item-promotion.model";

export interface PromotionModel {
  items: ItemPromotionModel[];
  categories: CategoryPromotionModel[];
  bogoPromotions: BogoPromotionModel[];
  bulkPromotions: BulkPromotionModel[];
}
