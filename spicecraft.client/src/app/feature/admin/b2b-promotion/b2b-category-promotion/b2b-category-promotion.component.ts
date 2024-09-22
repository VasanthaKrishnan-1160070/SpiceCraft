import {Component, OnInit} from '@angular/core';
import {DxDataGridModule, DxFormModule, DxPopupModule} from "devextreme-angular";
import {AsyncPipe, CommonModule} from "@angular/common";
import {Observable} from "rxjs";
import {CategoryPromotionModel} from "../../../../core/model/promotion/category-promotion.model";
import {PromotionService} from "../../../../core/service/promotion.service";
import {map} from "rxjs/operators";
import { confirm } from 'devextreme/ui/dialog';
import notify from 'devextreme/ui/notify';

@Component({
  selector: 'sc-b2b-category-promotion',
  standalone: true,
  imports: [
    DxDataGridModule,
    AsyncPipe,
    CommonModule,
    DxPopupModule,
    DxFormModule
  ],
  templateUrl: './b2b-category-promotion.component.html',
  styleUrl: './b2b-category-promotion.component.css'
})
export class B2bCategoryPromotionComponent implements OnInit {
  categoryPromotions$!: Observable<CategoryPromotionModel[]>;
  showAddPromotionPopup = false;
  selectedItem: CategoryPromotionModel = {
    categoryId: 0,
    categoryName: '',
    parentCategoryName: '',
    discountRate: '0%',
    hasPromotion: 'No',
    actionHidden: '',
    actionAdd: true,
    actionRemove: false,
  };
  constructor(private promotionService: PromotionService) {}

  ngOnInit(): void {
    this.loadCategoryPromotions();
  }

  loadCategoryPromotions(): void {
    this.categoryPromotions$ = this.promotionService.getPromotions().pipe(
      map((result) => result.data?.categories as CategoryPromotionModel[])
    );
  }

  // Remove individual category promotion
  removeCategoryPromotion(categoryId: number): void {
    const confirmation = confirm('Are you sure you want to remove this promotion?', 'Remove Promotion');
    confirmation.then((dialogResult) => {
      if (dialogResult) {
        this.promotionService.removeCategoryPromotion(categoryId).subscribe((response) => {
          if (response.isSuccess) {
            notify('Category promotion removed successfully', 'success', 2000);
            this.loadCategoryPromotions(); // Reload the grid data
          } else {
            notify('Failed to remove category promotion', 'error', 2000);
          }
        });
      }
    });
  }

  // Remove all category promotions
  removeAllCategoryPromotions(): void {
    const confirmation = confirm('Are you sure you want to remove all promotions?', 'Remove All Promotions');
    confirmation.then((dialogResult) => {
      if (dialogResult) {
        this.promotionService.removeAllCategoryPromotions().subscribe((response) => {
          if (response.isSuccess) {
            notify('All category promotions removed successfully', 'success', 2000);
            this.loadCategoryPromotions(); // Reload the grid data
          } else {
            notify('Failed to remove all category promotions', 'error', 2000);
          }
        });
      }
    });
  }

  // Show add promotion popup
  showAddPromotionModal(item: CategoryPromotionModel): void {
    this.selectedItem = item;
    this.showAddPromotionPopup = true;
  }

  // Handle adding a new category promotion
  addCategoryPromotion(): void {
    this.promotionService.addCategoryPromotion(this.selectedItem).subscribe((response) => {
      if (response.isSuccess) {
        notify('Category promotion added successfully', 'success', 2000);
        this.loadCategoryPromotions(); // Reload the grid
        this.showAddPromotionPopup = false;
      } else {
        notify('Failed to add category promotion', 'error', 2000);
      }
    });
  }
}
