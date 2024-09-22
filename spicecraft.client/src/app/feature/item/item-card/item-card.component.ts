import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import {ItemSummaryModel} from "../../../core/model/item/item-summary-item.model";
import {MenuItemModel} from "../../../core/model/item/menu-item.model";
import {RouterLink} from "@angular/router";
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {DxButtonModule, DxFormModule, DxPopupModule} from "devextreme-angular";
import {UserService} from "../../../core/service/user.service";
import {AuthService} from "../../../core/service/auth.service";
import {ItemService} from "../../../core/service/item.service";
import DevExpress from "devextreme";
import notify from 'devextreme/ui/notify';

@Component({
  selector: 'sc-item-card',
  standalone: true,
  imports: [
    RouterLink,
    NgClass,
    NgIf,
    FormsModule,
    NgForOf,
    DxPopupModule,
    DxFormModule,
    DxButtonModule
  ],
  templateUrl: './item-card.component.html',
  styleUrl: './item-card.component.css'
})
export class ItemCardComponent {
  @Input() menuItem!: MenuItemModel;
  @Input() showAddToCart: boolean = false;
  @Input() productSizes: Array<{key: string, value: string}> = [];
  @Input() productColors: Array<{key: string, value: string}> = [];
  @Output() addToCart: EventEmitter<number> = new EventEmitter<number>();

  private _userService = inject(UserService);
  private _authService = inject(AuthService);
  private _itemService = inject(ItemService);

  quantity: number = 1;
  selectedSize: string = 'L';
  selectedColor: string = 'Black';

  get isUserAuthenticated(): boolean {
    // Replace with actual logic to check if user is authenticated
    return this._authService.isAuthenticated();
  }

  get isUserCorporateClient(): boolean {
    // Replace with actual logic to check corporate client status
    return false;
  }

  get isUserInternal(): boolean {
    // Replace with actual logic to check if the user is internal
    return this._authService.isInternalUser();
  }

  get productImage(): string {
    return 'assets/images/products/' +  'product_image.jpg';
  }

  get saleClass(): string {
    return (this.menuItem.isInSale === 'Yes' && !this.isUserCorporateClient ? 'visible' : 'invisible') + ' d-flex flex-row justify-content-between pe-3 pt-2';
  }

  get showDiscountRate(): boolean {
    return !!this.menuItem.discountRate && !this.isUserCorporateClient;
  }

  get showDiscountPrice(): boolean {
    return !!this.menuItem.discountRate && !!this.menuItem.discountPrice && !this.isUserCorporateClient;
  }

  get showBulkDiscount(): boolean {
    return this.isUserCorporateClient && !!this.menuItem.bulkDiscountRate && !!this.menuItem.bulkDiscountRequiredQuantity;
  }

  addItemToCart(): void {
    // Implement the logic to add the product to the cart
    console.log("Add to Cart clicked");
    this.addToCart.emit(this.menuItem.itemId);
  }

  removeProductFromListing(itemId: number): void {
    // Implement the logic to remove the product from the listing
    console.log(`Removing product with id: ${itemId}`);
    this._itemService.addOrRemoveItemFromListing(itemId, false).subscribe(
      s => {
        notify('Item is removed from the listing successfully', 'success');
        this.menuItem.isRemoved = true;
      }
    );
  }

  addProductToListing(itemId: number): void {
    // Implement the logic to add the product back to the listing
    console.log(`Adding product with id: ${itemId} to the listing`);
    this._itemService.addOrRemoveItemFromListing(itemId).subscribe(
      s => {
        notify('Item is add back to the listing successfully', 'success');
        this.menuItem.isRemoved = false;
      }
    );

  }
}
