import {Component, inject, input, Input, OnDestroy, OnInit} from '@angular/core';
import {DxButtonModule, DxFormModule, DxPopupModule, DxTemplateModule} from "devextreme-angular";
import {
  DxiItemModule,
  DxiValidationRuleModule,
  DxoLabelModule,
  DxoValidationModule
} from "devextreme-angular/ui/nested";
import {CartService} from "../../../core/service/cart.service";
import {BehaviorSubject, of, Subject} from "rxjs";
import {AsyncPipe, NgIf} from "@angular/common";
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";
import {take, takeUntil} from "rxjs/operators";
import {FormsModule} from "@angular/forms";
import {CreateUpdateCartItemRequest} from "../../../core/model/cart/create-update-cart-item-request.model";
import {UserService} from "../../../core/service/user.service";
import DevExpress from "devextreme";
import notify from 'devextreme/ui/notify';

@Component({
  selector: 'sc-add-to-cart',
  standalone: true,
  imports: [
    DxButtonModule,
    DxFormModule,
    DxPopupModule,
    DxTemplateModule,
    DxiItemModule,
    DxiValidationRuleModule,
    DxoLabelModule,
    AsyncPipe,
    NgIf,
    FormsModule,
    DxoValidationModule,
    DxiValidationRuleModule
  ],
  templateUrl: './add-to-cart.component.html',
  styleUrl: './add-to-cart.component.css'
})
export class AddToCartComponent implements OnInit, OnDestroy {

  itemId = input<number>(0);
  private _cartService = inject(CartService);
  private _userService = inject(UserService);
  private destroy$: Subject<void> = new Subject<void>();

  public ngOnInit() {
    this._cartService.showAddToCartDialog$.pipe(
     takeUntil(this.destroy$)
    )
    .subscribe(showAddToCart => {
      this.isPopupVisible = showAddToCart;
    });
  }

  isPopupVisible: boolean = false;
  formData: any = {
    quantity: 1,
    selectedSize: null,
    selectedColor: null
  };

  productSizes = [
    { key: 'S', value: 'Small' },
    { key: 'M', value: 'Medium' },
    { key: 'L', value: 'Large' }
  ];

  productColors = [
    { key: 'R', value: 'Red' },
    { key: 'G', value: 'Green' },
    { key: 'B', value: 'Blue' }
  ];

  // Opens the Popup
  openPopup() {
    this.isPopupVisible = true;
  }

  // Handles Popup Close
  onPopupHiding() {
   this.closePopup();
  }

  closePopup() {
    this.isPopupVisible = false;
    // this._cartService.showAddToCartDialog$.next(this.isPopupVisible);
  }

  // Handles form submission
  addToCart() {
    if (this.formData.quantity && this.formData.selectedSize && this.formData.selectedColor) {
       const createUpdateCartItem: CreateUpdateCartItemRequest = {
         itemId: this.itemId(),
         quantity: this.formData.quantity,
         size: this.formData.size,
         spiceLevel: 'Medium',
         userId: 0,
         description: '',
       }

       this._cartService.addToCart(this.formData.quantity, this.formData.size,'', this.itemId()).pipe(
        take(1)
       )
         .subscribe(res => {
           this.closePopup();
           notify('Item is added to cart successfully', 'success');
         });
    }
  }

  public ngOnDestroy() {
    this.destroy$.next();
  }
}
