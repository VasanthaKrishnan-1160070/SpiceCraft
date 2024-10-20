import {Component, inject, OnInit} from '@angular/core';
import {DxButtonModule, DxDataGridModule, DxFormModule, DxPopupModule} from "devextreme-angular";
import {TitleComponent} from "../../../../shared/components/title/title.component";
import {IngredientInventoryModel} from "../../../../core/model/inventory/ingredient-inventory.model";
import {Observable} from "rxjs";
import {InventoryService} from "../../../../core/service/inventory.service";
import {UpdateStockModel} from "../../../../core/model/inventory/update-stock.model";
import {map} from "rxjs/operators";
import {AsyncPipe} from "@angular/common";
import {NotifyService} from "../../../../core/service/notify.service";

@Component({
  selector: 'sc-b2b-ingredient-iventory',
  standalone: true,
  imports: [
    DxButtonModule,
    DxFormModule,
    DxDataGridModule,
    TitleComponent,
    DxPopupModule,
    AsyncPipe
  ],
  templateUrl: './b2b-ingredient-iventory.component.html',
  styleUrl: './b2b-ingredient-iventory.component.css'
})
export class B2bIngredientIventoryComponent implements OnInit {
  inventory$!: Observable<IngredientInventoryModel[]>;
  editPopupVisible = false;
  selectedIngredient!: IngredientInventoryModel;

  private _notifyService = inject(NotifyService);

  constructor(private inventoryService: InventoryService) {}

  ngOnInit(): void {
    this.loadInventory();
  }

  loadInventory(): void {
    this.inventory$ = this.inventoryService.getInventory().pipe(
      map((result) =>  result.data as IngredientInventoryModel[])
    )
  }

  openEditPopup(ingredient: IngredientInventoryModel): void {
    this.selectedIngredient = { ...ingredient };
    this.editPopupVisible = true;
  }

  closeEditPopup(): void {
    this.editPopupVisible = false;
  }

  saveStock(): void {
    const updateStockDTO: UpdateStockModel = {
      ingredientId: this.selectedIngredient.ingredientId,
      newStock: this.selectedIngredient.currentStock
    };

    this.inventoryService.updateStock(updateStockDTO).subscribe(() => {
      this._notifyService.showSuccess("Ingredients Updated Successfully");
      this.loadInventory(); // Refresh the inventory after saving
      this.closeEditPopup();
    });
  }
}
