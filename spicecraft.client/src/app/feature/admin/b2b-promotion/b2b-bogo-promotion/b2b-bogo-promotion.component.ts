import {Component, OnInit} from '@angular/core';
import {PromotionService} from "../../../../core/service/promotion.service";
import {Observable} from "rxjs";
import {BogoPromotionModel} from "../../../../core/model/promotion/bogo-promotion.model";
import {map} from "rxjs/operators";
import {DxButtonModule, DxDataGridModule, DxFormModule, DxPopupModule} from "devextreme-angular";
import {AsyncPipe, CommonModule} from "@angular/common";
import { confirm } from 'devextreme/ui/dialog';
import notify from 'devextreme/ui/notify';
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'sc-b2b-bogo-promotion',
  standalone: true,
  imports: [
    DxDataGridModule,
    AsyncPipe,
    CommonModule,
    DxPopupModule,
    FormsModule,
    DxFormModule,
    DxButtonModule
  ],
  templateUrl: './b2b-bogo-promotion.component.html',
  styleUrl: './b2b-bogo-promotion.component.css'
})
export class B2bBogoPromotionComponent implements OnInit {
  bogoPromotions$!: Observable<BogoPromotionModel[]>;
  showAddPromotionPopup = false;
  selectedItem: BogoPromotionModel = {
    itemId: 0,
    itemName: '',
    categoryName: '',
    actualPrice: '',
    comboName: '',
    buyQuantity: '',
    getQuantity: '',
  };

  constructor(private promotionService: PromotionService) {}

  ngOnInit(): void {
    this.loadBogoPromotions();
  }

  loadBogoPromotions(): void {
    this.bogoPromotions$ = this.promotionService.getPromotions().pipe(
      map((result) => result.data?.bogoPromotions as BogoPromotionModel[])
    );
  }

  // Remove individual BOGO promotion
  removeBogoPromotion(itemId: number): void {
    const confirmation = confirm('Are you sure you want to remove this promotion?', 'Remove BOGO Promotion');
    confirmation.then((dialogResult) => {
      if (dialogResult) {
        this.promotionService.removeBogoPromotion(itemId).subscribe((response) => {
          if (response.isSuccess) {
            notify('BOGO promotion removed successfully', 'success', 2000);
            this.loadBogoPromotions(); // Reload the grid data
          } else {
            notify('Failed to remove BOGO promotion', 'error', 2000);
          }
        });
      }
    });
  }

  // Remove all BOGO promotions
  removeAllBogoPromotions(): void {
    const confirmation = confirm('Are you sure you want to remove all promotions?', 'Remove All BOGO Promotions');
    confirmation.then((dialogResult) => {
      if (dialogResult) {
        this.promotionService.removeAllBogoPromotions().subscribe((response) => {
          if (response.isSuccess) {
            notify('All BOGO promotions removed successfully', 'success', 2000);
            this.loadBogoPromotions(); // Reload the grid data
          } else {
            notify('Failed to remove all BOGO promotions', 'error', 2000);
          }
        });
      }
    });
  }

  // Show add promotion popup
  showAddPromotionModal(item: BogoPromotionModel): void {
    this.selectedItem = item;
    this.showAddPromotionPopup = true;
  }

  // Handle adding a new BOGO promotion
  addBogoPromotion(): void {
    if (this.selectedItem.buyQuantity) {
      this.selectedItem.buyQuantity = this.selectedItem.buyQuantity.toString();
    }

    if (this.selectedItem.getQuantity) {
      this.selectedItem.getQuantity = this.selectedItem.getQuantity.toString();
    }


    this.promotionService.addBogoPromotion(this.selectedItem).subscribe((response) => {
      if (response.isSuccess) {
        notify('BOGO promotion added successfully', 'success', 2000);
        this.loadBogoPromotions(); // Reload the grid
        this.showAddPromotionPopup = false;
      } else {
        notify('Failed to add BOGO promotion', 'error', 2000);
      }
    });
  }
}
