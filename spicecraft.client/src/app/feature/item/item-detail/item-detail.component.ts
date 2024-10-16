import {Component, inject, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {
  DxButtonModule, DxFileUploaderComponent,
  DxFileUploaderModule, DxFormComponent,
  DxFormModule, DxGalleryModule,
  DxSelectBoxModule, DxValidatorComponent,
  DxValidatorModule
} from "devextreme-angular";
import {FormsModule} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {CommonModule, NgIf, Location} from "@angular/common";
import {ItemDetailModel} from "../../../core/model/item/item-detail.model";
import {ItemService} from "../../../core/service/item.service";
import {ActivatedRoute, ActivatedRouteSnapshot, Router} from "@angular/router";
import {sample, Subject} from "rxjs";
import {take, takeUntil} from "rxjs/operators";
import {CategoryModel} from "../../../core/model/category/category.model";
import {ItemImageModel} from "../../../core/model/item/item-image.model";
import {ItemSummaryModel} from "../../../core/model/item/item-summary-item.model";
import {CategoryService} from "../../../core/service/category.service";
import {NotifyService} from "../../../core/service/notify.service";
import {environment} from "../../../../environments/environment";
import {ImageUrlPipe} from "../../../shared/pipes/image-url.pipe";
import {CreateUpdateItemModel} from "../../../core/model/item/create-update-item.model";
import { ChangeDetectorRef } from '@angular/core';
import {AuthService} from "../../../core/service/auth.service";
import {TitleComponent} from "../../../shared/components/title/title.component";
import {UserItemRatingComponent} from "../user-item-rating/user-item-rating.component";

@Component({
  selector: 'sc-item-detail',
  standalone: true,
  imports: [
    DxFormModule,
    FormsModule,
    NgIf,
    CommonModule,
    DxButtonModule,
    DxFileUploaderModule,
    ImageUrlPipe,
    DxSelectBoxModule,
    DxValidatorModule,
    DxGalleryModule,
    TitleComponent,
    UserItemRatingComponent
  ],
  templateUrl: './item-detail.component.html',
  styleUrl: './item-detail.component.css'
})
export class ItemDetailComponent implements  OnInit, OnDestroy{

  @ViewChild(DxFormComponent) formInstance!: DxFormComponent;
  @ViewChild('mainImageValidator') mainImageValidator!: DxValidatorComponent;
  @ViewChild('itemImagesUploader') itemImagesUploader!: DxFileUploaderComponent;

  public itemModel!: ItemDetailModel;
  public itemSummary!: ItemSummaryModel;
  private createUpdateItemModel!: CreateUpdateItemModel;
  private _itemService: ItemService = inject(ItemService);
  private _notifyService = inject(NotifyService);
  private _location = inject(Location);
  private _activatedRoute = inject(ActivatedRoute);
  private _router = inject(Router);
  private _categoryService = inject(CategoryService);
  private _destroy$: Subject<void> = new Subject<void>();
  private _cdr: ChangeDetectorRef = inject(ChangeDetectorRef);
  private _authService = inject(AuthService);

  selectedFiles: File[] = [];
  allImages: any[] = [];
  selectedMainImage: string | null = null;  // Store the main image file name
  isImageRequired: boolean = false;  // for validation logic
  isImageValid: boolean = false;     // to show valid feedback
  removedImages: string[] = [];  // To track removed images
  galleryImages: any[] = [];



  categories: CategoryModel[] = [];
  subCategories: CategoryModel[] = [];
  imagePreviews: any[] = [];  // Array to store the local image URLs
  itemImages: ItemImageModel[] = [];
  isUserInternal: boolean = this._authService.isInternalUser(); // Set based on user role
  isCustomer = this._authService.isUserCustomer();
  categoryName!: string;
  subCategoryName!: string;




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
          this.itemImages = s.data?.productImages as ItemImageModel[] || [];

          this.selectedMainImage = this.itemImages?.find(f => f.isMain)?.imageName as string;
          // now adding the allImages which will be shown in the lookup
          this.itemImages.forEach((item: ItemImageModel) => {
              this.allImages.push({name: item.imageName});
              this.galleryImages.push({imageAlt: item.imageName, imageCode: item.imageCode , imageSrc: `${environment.backend_url}/uploads/items/${item.imageCode}`});
          });


          this.categoryName = this.itemSummary.categoryName;

          const subCategory = this.subCategories.find((subCat) => subCat.categoryId === this.itemSummary.subCategoryId);
          this.subCategoryName = subCategory ? subCategory.categoryName : 'N/A';
        }
      )
  }

  onImageUpload(event: any) {
    this.selectedFiles = event.value;
    this.selectedMainImage = null;  // Reset main image selection

    this.imagePreviews = [];
    // Generate image previews using URL.createObjectURL
    this.selectedFiles.forEach((file, index) => {
      const objectUrl = URL.createObjectURL(file);
      this.imagePreviews.push({
        url: objectUrl,     // Image URL for preview
        name: file.name,    // File name for display
        index: index        // Index for removing or referencing the file
      });

      // adding the selected image to the all image if does not exist
      if (!this.allImages?.find(f => f.name === file.name)) {
        this.allImages.push({name: file.name});
      }
    });

    this.selectedMainImage = null;  // Reset main image selection

    if (this.selectedFiles.length > 0) {
      this.isImageValid = true;
      this.isImageRequired = false;
    } else {
      this.isImageRequired = true;
      this.isImageValid = false;
    }
  }

  onMainImageSelect(fileName: string) {
    this.selectedMainImage = fileName;  // Set the selected file as the main image
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

  removePreviewImage(index: number) {
    const imgToRemove = this.selectedFiles[index];

    // remove it from the all image
    this.allImages = this.allImages.filter((img) => img.name !== imgToRemove.name);

    this.selectedFiles.splice(index, 1);
    this.imagePreviews = this.imagePreviews.filter((img) => img.index !== index);

    // If the removed image was the selected main image, reset the main image selection
    if (this.selectedMainImage === this.selectedFiles[index]?.name) {
      this.selectedMainImage = null;
    }

    // Force Angular to detect changes after the list is updated
    this._cdr.detectChanges();
  }

  // Remove image by imageCode and add to removedImages list
  removeImage(imageCode: string, imageName: string): void {

    // remove it from the all image
    this.allImages = this.allImages.filter((img) => img.name !== imageName);

    const removedImageIndex = this.itemImages.findIndex(img => img.imageCode === imageCode);
    if (removedImageIndex > -1) {
      this.removedImages.push(imageCode);  // Track the removed image by its imageCode
      this.itemImages.splice(removedImageIndex, 1);  // Remove the image from the array
    }
  }

  onCancel() {
    // Redirect or reset the form
    this._location.back();
  }

  onSave(event: any) {

    // Trigger form validation
    const validationResults = this.formInstance.instance.validate();
    const mainImageValidation = this.mainImageValidator.instance.validate();

    // Custom validation for the file uploader (check if images are selected)
    this.isImageRequired = this.selectedFiles.length === 0;

    // Check if all validations pass
    if (!validationResults.isValid || !mainImageValidation.isValid) {
      this._notifyService.showError("Please fix the validation errors and try again");
      console.error('Validation failed');
      return;
    }

    // Submit form data
    this.createUpdateItemModel = {
      itemSummary: this.itemSummary,
      itemImages: this.itemImages,
      removedImages: this.removedImages,
      mainImageName: this.selectedMainImage
    };

    this._itemService.saveItemDetails(this.createUpdateItemModel, this.selectedFiles).pipe(
      take(1),
      takeUntil(this._destroy$)
    )
    .subscribe(s => {
      this._notifyService.showSuccess("Item Successfully Saved");
      this.selectedFiles = [];
      this.imagePreviews = [];
      this.allImages = [];
      this.selectedMainImage = null;
      this.loadProductDetails(this.itemSummary.itemId);
    })
  }

  ngOnDestroy() {
  }

  protected readonly sample = sample;
}
