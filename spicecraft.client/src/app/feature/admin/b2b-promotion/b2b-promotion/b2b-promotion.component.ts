import { Component } from '@angular/core';
import {DxTabPanelModule} from "devextreme-angular";
import {B2bBogoPromotionComponent} from "../b2b-bogo-promotion/b2b-bogo-promotion.component";
import {B2bCategoryPromotionComponent} from "../b2b-category-promotion/b2b-category-promotion.component";
import {B2bItemPromotionComponent} from "../b2b-item-promotion/b2b-item-promotion.component";
import {B2bBulkPromotionComponent} from "../b2b-bulk-promotion/b2b-bulk-promotion.component";
import {CommonModule} from "@angular/common";
import {TitleComponent} from "../../../../shared/components/title/title.component";

@Component({
  selector: 'sc-b2b-promotion',
  standalone: true,
  imports: [
    DxTabPanelModule,
    B2bBogoPromotionComponent,
    B2bCategoryPromotionComponent,
    B2bItemPromotionComponent,
    B2bBulkPromotionComponent,
    CommonModule,
    TitleComponent
  ],
  templateUrl: './b2b-promotion.component.html',
  styleUrl: './b2b-promotion.component.css'
})
export class B2bPromotionComponent {
  promotionTabs: any[] = [
    { title: 'Item Promotions', template: 'itemPromotion' },
    { title: 'Category Promotions', template: 'categoryPromotion' },
    { title: 'BOGO Promotions', template: 'bogoPromotion' },
    { title: 'Bulk Promotions', template: 'bulkPromotion' },
  ];
}
