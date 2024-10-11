import {ItemSummaryModel} from "./item-summary-item.model";
import {ItemImageModel} from "./item-image.model";

export interface CreateUpdateItemModel {
  itemSummary: ItemSummaryModel;
  itemImages: ItemImageModel[];
  removedImages: string[];
  mainImageName: string | null;
}
