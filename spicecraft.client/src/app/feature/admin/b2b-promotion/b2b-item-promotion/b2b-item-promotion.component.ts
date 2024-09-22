import {Component, OnInit} from '@angular/core';
import {map} from "rxjs/operators";
import {ItemPromotionModel} from "../../../../core/model/promotion/item-promotion.model";
import {Observable} from "rxjs";
import {PromotionService} from "../../../../core/service/promotion.service";
import {DxDataGridModule, DxFormModule, DxPopupModule} from "devextreme-angular";
import {AsyncPipe, CommonModule} from "@angular/common";
import { confirm } from 'devextreme/ui/dialog';
import notify from 'devextreme/ui/notify';

@Component({
  selector: 'sc-b2b-item-promotion',
  templateUrl: './b2b-item-promotion.component.html',
  styleUrl: './b2b-item-promotion.component.css',
  imports: [
    DxDataGridModule,
    AsyncPipe,
    CommonModule,
    DxPopupModule,
    DxFormModule
  ],
  standalone: true
})
export class B2bItemPromotionComponent implements OnInit {
  itemPromotions$!: Observable<ItemPromotionModel[]>;
  showAddPromotionPopup = false;
  selectedItem: ItemPromotionModel = {
    itemId: 0,
    itemName: '',
    categoryName: '',
    actualPrice: '',
    discountRate: '0',
    priceAfterDiscount: '',
    hasPromotion: 'No',
    actionHidden: '',
    actionAdd: true,
    actionRemove: false,
  };
  constructor(private promotionService: PromotionService) {}

  ngOnInit(): void {
    this.loadItemPromotions();
  }

  loadItemPromotions(): void {
    this.itemPromotions$ = this.promotionService.getPromotions().pipe(
      map((result) => result?.data?.items as ItemPromotionModel[] )
    );
  }

  // Remove individual item promotion
  removeItemPromotion(itemId: number): void {
    const confirmation = confirm('Are you sure you want to remove this promotion?', 'Remove Promotion');
    confirmation.then((dialogResult) => {
      if (dialogResult) {
        this.promotionService.removeItemPromotion(itemId).subscribe((response) => {
          if (response.isSuccess) {
            notify('Item promotion removed successfully', 'success', 2000);
            this.loadItemPromotions(); // Reload the grid data
          } else {
            notify('Failed to remove item promotion', 'error', 2000);
          }
        });
      }
    });
  }

  // Remove all item promotions
  removeAllItemPromotions(): void {
    const confirmation = confirm('Are you sure you want to remove all promotions?', 'Remove All Promotions');
    confirmation.then((dialogResult) => {
      if (dialogResult) {
        this.promotionService.removeAllItemPromotions().subscribe((response) => {
          if (response.isSuccess) {
            notify('All item promotions removed successfully', 'success', 2000);
            this.loadItemPromotions(); // Reload the grid data
          } else {
            notify('Failed to remove all item promotions', 'error', 2000);
          }
        });
      }
    });
  }

  // Show add promotion popup
  showAddPromotionModal(item: ItemPromotionModel): void {
    this.selectedItem = item;
    this.showAddPromotionPopup = true;
  }

  // Handle adding a new item promotion
  addItemPromotion(): void {
    this.promotionService.addItemPromotion(this.selectedItem).subscribe((response) => {
      if (response.isSuccess) {
        notify('Item promotion added successfully', 'success', 2000);
        this.loadItemPromotions(); // Reload the grid
        this.showAddPromotionPopup = false;
      } else {
        notify('Failed to add item promotion', 'error', 2000);
      }
    });
  }
}
