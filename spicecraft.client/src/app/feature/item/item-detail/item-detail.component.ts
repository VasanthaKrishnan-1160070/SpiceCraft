import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {DxFormModule} from "devextreme-angular";
import {FormsModule} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {CommonModule, NgIf, Location} from "@angular/common";
import {ItemDetailModel} from "../../../core/model/item/item-detail.model";
import {ItemService} from "../../../core/service/item.service";
import {ActivatedRoute, ActivatedRouteSnapshot, Router} from "@angular/router";
import {Subject} from "rxjs";
import {take, takeUntil} from "rxjs/operators";
import {CategoryModel} from "../../../core/model/category/category.model";
import {ItemImageModel} from "../../../core/model/item/item-image.model";
import {ItemSummaryModel} from "../../../core/model/item/item-summary-item.model";
import {CategoryService} from "../../../core/service/category.service";

@Component({
  selector: 'sc-item-detail',
  standalone: true,
  imports: [
    DxFormModule,
    FormsModule,
    NgIf,
    CommonModule
  ],
  templateUrl: './item-detail.component.html',
  styleUrl: './item-detail.component.css'
})
export class ItemDetailComponent implements  OnInit, OnDestroy{
  public itemModel!: ItemDetailModel;
  public itemSummary!: ItemSummaryModel;
  private _itemService: ItemService = inject(ItemService);
  private _location = inject(Location);
  private _activatedRoute = inject(ActivatedRoute);
  private _router = inject(Router);
  private _categoryService = inject(CategoryService);
  private _destroy$: Subject<void> = new Subject<void>();



  categories: CategoryModel[] = [];
  subCategories: CategoryModel[] = [];
  itemImages!: ItemImageModel[];
  isUserInternal: boolean = true; // Set based on user role



  ngOnInit() {
    this._activatedRoute.paramMap
      .pipe(takeUntil(this._destroy$))
      .subscribe(params => {
        const itemId = +(params.get('itemId') || 0);
        this.loadProductDetails(itemId);
      });
  }


  loadProductDetails(itemId: number) {
    this._itemService.getItemDetailById(itemId).pipe(
      takeUntil(this._destroy$)
    )
      .subscribe(
        s => {
          this.itemModel = s.data as ItemDetailModel;
          this.itemSummary = s.data?.productDetails as ItemSummaryModel;
          this.categories = s.data?.categories as CategoryModel[];
          this.subCategories = s.data?.subCategories as CategoryModel[];
          this.itemImages = s.data?.productImages as ItemImageModel[];
        }
      )
  }

  onParentCategoryChanged = (event: any) => {
    const selectedVal = event.value;
    this._categoryService
       .getSubCategories(selectedVal).pipe(
         takeUntil(this._destroy$)
    )
      .subscribe(s => {
        this.subCategories = s;
      })
  }

  imgUrl(imageCode: string) {
    return `/assets/images/products/${imageCode}`;
  }

  onImageUpload(event: any) {
    const files = event.target.files;
    // Handle image upload logic here
  }

  onCancel() {
    // Redirect or reset the form
  }

  onSave(event: any) {
    // Submit form data
    // this.http.post('/api/product/save', this.productModel).subscribe((result: any) => {
    //   // Handle the response after saving
    // });
  }

  ngOnDestroy() {
  }
}
