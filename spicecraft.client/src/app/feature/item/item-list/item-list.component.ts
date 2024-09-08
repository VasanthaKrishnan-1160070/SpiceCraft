import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {ItemService} from "../../../core/service/item.service";
import {map, take, takeUntil} from "rxjs/operators";
import {Subject} from "rxjs";
import {MenuItemModel} from "../../../core/model/item/menu-item.model";
import {ItemCardComponent} from "../item-card/item-card.component";
import {NgIf} from "@angular/common";
import {DxSelectBoxModule, DxTextBoxModule} from "devextreme-angular";
import {DxoLabelModule} from "devextreme-angular/ui/nested";
import {ItemFilterModel} from "../../../core/model/item/item-filter.model";
import {FormsModule} from "@angular/forms";
import {LookupService} from "../../../core/service/lookup.service";
import {LookupModel} from "../../../core/model/lookup.model";
import {CategoryService} from "../../../core/service/category.service";
import {CategoryModel} from "../../../core/model/category/category.model";
import {ItemFilterEnum} from "../../../core/enum/item-filter.enum";
import {ItemFilterSortingEnum} from "../../../core/enum/item-filter-sorting.enum";

@Component({
  selector: 'sc-item-list',
  standalone: true,
  imports: [
    TitleComponent,
    ItemCardComponent,
    NgIf,
    DxTextBoxModule,
    DxSelectBoxModule,
    DxoLabelModule,
    FormsModule
  ],
  templateUrl: './item-list.component.html',
  styleUrl: './item-list.component.css'
})
export class ItemListComponent implements OnInit, OnDestroy {
  private _itemService = inject(ItemService);
  private _categoryService = inject(CategoryService);
  private _lookupService = inject(LookupService);
  private _destroy$: Subject<void> = new Subject<void>();

  public menuItems!: MenuItemModel[] | null | undefined;
  public isUserExternal = true;
  public filterForm!: ItemFilterModel;
  public categories!: LookupModel[];
  public productFilters!: LookupModel[];
  public productSorting!: LookupModel[];
  public subCategories!: LookupModel[];

  ngOnInit() {
    this._itemService.getItems().pipe(
      takeUntil(this._destroy$)
    )
    .subscribe(
      s =>  {
        console.log(s);
        this.menuItems = s.data as any;
      }
    );

    this.getCategories();
    this.productFilters = this._lookupService.enumToDataSource(ItemFilterEnum);
    this.productSorting = this._lookupService.enumToDataSource(ItemFilterSortingEnum);
    this.filterForm = {
      keyword: '',
      categoryId: 0,
      subCategoryId: 0,
      filter: ItemFilterEnum.InStock,
      sorting: ItemFilterSortingEnum.NameAToZ,
      includeRemovedProducts: true
    }
  }

  public getCategories() {
    this._categoryService.getCategories()
      .pipe(
        map((categories: CategoryModel[]) =>
          categories.map(category => ({
            key: category.categoryId,
            value: category.categoryName
          }))
        ),
        takeUntil(this._destroy$)
      )
      .subscribe((mappedCategories) => {
        this.categories = mappedCategories;
      });
  }

  public getSubCategories() {
    const selectedCategory = this.filterForm.categoryId;

    this._categoryService.getSubCategories(selectedCategory)
      .pipe(
        map((subCategories: CategoryModel[]) =>
          subCategories.map(subCategory => ({
            key: subCategory.categoryId,
            value: subCategory.categoryName
          }))
        ),
        takeUntil(this._destroy$)
      )
      .subscribe((mappedSubCategories) => {
        this.subCategories = mappedSubCategories;  // Assign the mapped categories to subCategories
      });
  }

  public applyFilter() {
    this._itemService.getItems().pipe(
     take(1)
    )
      .subscribe(
        s =>  {
          this.menuItems = s.data as any;
        })
  }

  public resetFilter() {

  }

  ngOnDestroy() {
    this._destroy$.next();
  }
}
