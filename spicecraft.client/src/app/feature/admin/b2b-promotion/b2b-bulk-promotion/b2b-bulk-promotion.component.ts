import {Component, OnInit} from '@angular/core';
import {PromotionService} from "../../../../core/service/promotion.service";
import {BulkPromotionModel} from "../../../../core/model/promotion/bulk-promotion.model";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {DxDataGridModule, DxFormModule, DxPopupModule} from "devextreme-angular";
import {AsyncPipe, CommonModule} from "@angular/common";
import { confirm } from 'devextreme/ui/dialog';
import notify from 'devextreme/ui/notify';
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'sc-b2b-bulk-promotion',
  standalone: true,
  imports: [
    DxDataGridModule,
    AsyncPipe,
    CommonModule,
    DxPopupModule,
    FormsModule,
    DxFormModule
  ],
  templateUrl: './b2b-bulk-promotion.component.html',
  styleUrl: './b2b-bulk-promotion.component.css'
})
export class B2bBulkPromotionComponent implements OnInit {
  bulkPromotions$!: Observable<BulkPromotionModel[]>;
  showAddPromotionPopup = false;
  selectedItem: BulkPromotionModel = {
    itemId: 0,
    itemName: '',
    categoryName: '',
    actualPrice: '',
    requiredQuantityForPromotion: '',
    discountRate: '',
  };

  constructor(private promotionService: PromotionService) {}

  ngOnInit(): void {
    this.loadBulkPromotions();
  }

  loadBulkPromotions(): void {
    this.bulkPromotions$ = this.promotionService.getPromotions().pipe(
      map((result) => result.data?.bulkPromotions as BulkPromotionModel[])
    );
  }

  // Remove individual bulk promotion
  removeBulkPromotion(itemId: number): void {
    const confirmation = confirm('Are you sure you want to remove this promotion?', 'Remove Bulk Promotion');
    confirmation.then((dialogResult) => {
      if (dialogResult) {
        this.promotionService.removeBulkItemPromotion(itemId).subscribe((response) => {
          if (response.isSuccess) {
            notify('Bulk promotion removed successfully', 'success', 2000);
            this.loadBulkPromotions(); // Reload the grid data
          } else {
            notify('Failed to remove bulk promotion', 'error', 2000);
          }
        });
      }
    });
  }

  // Remove all bulk promotions
  removeAllBulkPromotions(): void {
    const confirmation = confirm('Are you sure you want to remove all promotions?', 'Remove All Bulk Promotions');
    confirmation.then((dialogResult) => {
      if (dialogResult) {
        this.promotionService.removeAllBulkItemPromotions().subscribe((response) => {
          if (response.isSuccess) {
            notify('All bulk promotions removed successfully', 'success', 2000);
            this.loadBulkPromotions(); // Reload the grid data
          } else {
            notify('Failed to remove all bulk promotions', 'error', 2000);
          }
        });
      }
    });
  }

  // Show add promotion popup
  showAddPromotionModal(item: BulkPromotionModel): void {
    this.selectedItem = item;
    this.showAddPromotionPopup = true;
  }

  // Handle adding a new bulk promotion
  addBulkPromotion(): void {

    if(this.selectedItem.requiredQuantityForPromotion) {
      this.selectedItem.requiredQuantityForPromotion = this.selectedItem.requiredQuantityForPromotion.toString();
    }

    this.promotionService.addBulkItemPromotion(this.selectedItem).subscribe((response) => {
      if (response.isSuccess) {
        notify('Bulk promotion added successfully', 'success', 2000);
        this.loadBulkPromotions(); // Reload the grid
        this.showAddPromotionPopup = false;
      } else {
        notify('Failed to add bulk promotion', 'error', 2000);
      }
    });
  }
}
