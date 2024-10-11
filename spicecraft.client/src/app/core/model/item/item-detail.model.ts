import {ItemSummaryModel} from "./item-summary-item.model";
import {CategoryModel} from "../category/category.model";
import {ItemImageModel} from "./item-image.model";

export interface ItemDetailModel {
  productDetails?: ItemSummaryModel | null;          // Nullable ProductSummaryDTO
  categories: CategoryModel[];                          // Array of CategoryDTO
  subCategories: CategoryModel[];                       // Array of CategoryDTO for subcategories
  productImages: ItemImageModel[];                      // Array of ItemImageDTO
  mainImageName: string | null;
}
