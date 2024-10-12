import { Component } from '@angular/core';
import {TitleComponent} from "../../../../shared/components/title/title.component";
import {DxButtonModule, DxDataGridModule, DxFormModule, DxPopupModule} from "devextreme-angular";
import {ProductInventoryModel} from "../../../../core/model/inventory/product-inventory.model";
import {InventoryService} from "../../../../core/service/inventory.service";
import {Observable} from "rxjs";
import notify from "devextreme/ui/notify";
import {map} from "rxjs/operators";
import {AsyncPipe} from "@angular/common";

@Component({
  selector: 'sc-b2b-inventory-list',
  standalone: true,
  imports: [
    TitleComponent,
    DxDataGridModule,
    AsyncPipe,
    DxFormModule,
    DxPopupModule,
    DxButtonModule
  ],
  templateUrl: './b2b-inventory-list.component.html',
  styleUrl: './b2b-inventory-list.component.css'
})
export class B2bInventoryListComponent {
  inventory$!: Observable<ProductInventoryModel[]>;
  editPopupVisible = false;
  selectedProduct: ProductInventoryModel | null = null;

  constructor(private inventoryService: InventoryService) {}

  ngOnInit(): void {
    this.loadInventory();
  }

  // Load inventory data
  loadInventory() {
    this.inventory$ = this.inventoryService.getAvailableProducts().pipe(
      map((result) =>  result.data as ProductInventoryModel[])
    );
  }

  // Open the edit popup for a product
  openEditPopup(product: ProductInventoryModel) {
    this.selectedProduct = { ...product }; // Clone the product to avoid direct modification
    this.editPopupVisible = true; // Open the popup
  }

  // Save the updated stock value
  saveStock() {
    if (this.selectedProduct) {
      this.inventoryService
        .updateProductStock(this.selectedProduct.itemId, this.selectedProduct.availableStock)
        .subscribe((response) => {
          if (response.isSuccess) {
            notify('Stock updated successfully', 'success', 2000);
            this.loadInventory(); // Refresh the inventory grid
          } else {
            notify(response.message, 'error', 2000);
          }
          this.closeEditPopup();
        });
    }
  }

  // Close the edit popup
  closeEditPopup() {
    this.editPopupVisible = false;
    this.selectedProduct = null;
  }
}
