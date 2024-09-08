import {ItemFilterEnum} from "../../enum/item-filter.enum";
import {ItemFilterSortingEnum} from "../../enum/item-filter-sorting.enum";

export interface ItemFilterModel {
  categoryId: number;                  // Corresponds to C# int
  subCategoryId: number;               // Corresponds to C# int
  keyword: string;                     // Corresponds to C# string
  filter: ItemFilterEnum;           // Corresponds to ProductFilterEnum enum
  sorting: ItemFilterSortingEnum;         // Corresponds to ProductSortingEnum enum
  includeRemovedProducts: boolean;     // Corresponds to C# bool
}
