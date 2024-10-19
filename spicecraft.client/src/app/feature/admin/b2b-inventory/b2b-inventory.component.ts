import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {B2bBogoPromotionComponent} from "../b2b-promotion/b2b-bogo-promotion/b2b-bogo-promotion.component";
import {B2bBulkPromotionComponent} from "../b2b-promotion/b2b-bulk-promotion/b2b-bulk-promotion.component";
import {B2bCategoryPromotionComponent} from "../b2b-promotion/b2b-category-promotion/b2b-category-promotion.component";
import {B2bItemPromotionComponent} from "../b2b-promotion/b2b-item-promotion/b2b-item-promotion.component";
import {DxTabPanelModule, DxTemplateModule} from "devextreme-angular";
import {B2bIngredientIventoryComponent} from "./b2b-ingredient-iventory/b2b-ingredient-iventory.component";
import {B2bInventoryListComponent} from "./b2b-inventory-list/b2b-inventory-list.component";
import {B2bIngredientPackComponent} from "./b2b-ingredient-pack/b2b-ingredient-pack.component";

@Component({
  selector: 'sc-b2b-inventory',
  standalone: true,
  imports: [
    TitleComponent,
    B2bBogoPromotionComponent,
    B2bBulkPromotionComponent,
    B2bCategoryPromotionComponent,
    B2bItemPromotionComponent,
    DxTabPanelModule,
    DxTemplateModule,
    B2bIngredientIventoryComponent,
    B2bInventoryListComponent,
    B2bIngredientPackComponent
  ],
  templateUrl: './b2b-inventory.component.html',
  styleUrl: './b2b-inventory.component.css'
})
export class B2bInventoryComponent {
  inventoryTabs: any[] = [
    { title: 'Ingredients Inventory', template: 'ingredientsInventory' },
    { title: 'Items Inventory', template: 'itemsInventory' }
  ];
}
