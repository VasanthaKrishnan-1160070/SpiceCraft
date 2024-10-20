import {Component, EventEmitter, inject, OnDestroy, OnInit, Output} from '@angular/core';
import {RecentlyViewedItemModel} from "../../../core/model/recentlyViewed/RecentlyViewedItemModel";
import {RecentlyViewedItemService} from "../../../core/service/recently-viewed-item.service";
import {Subject} from "rxjs";
import {take, takeUntil} from "rxjs/operators";
import {CarouselComponent} from "../../../shared/components/carousel/carousel.component";
import {MenuItemModel} from "../../../core/model/item/menu-item.model";
import {UserService} from "../../../core/service/user.service";
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-recently-viewed-items',
  standalone: true,
  imports: [
    CarouselComponent,
    TitleComponent
  ],
  templateUrl: './recently-viewed-items.component.html',
  styleUrl: './recently-viewed-items.component.css'
})
export class RecentlyViewedItemsComponent implements OnInit, OnDestroy {
  @Output() addToCart = new EventEmitter<number>();
  recentlyViewedItems: MenuItemModel[] = [];
  private _userService: UserService = inject(UserService);
  userId: number = 0; // Set to the currently logged-in user
  private _destroy$: Subject<void> = new Subject<void>();

  constructor(private recentlyViewedItemService: RecentlyViewedItemService) {}

  ngOnInit(): void {
    this.userId = this._userService.getCurrentUserId();
    this.loadRecentlyViewedItems();
  }

  loadRecentlyViewedItems(): void {
    const userId = this._userService.getCurrentUserId();
    this.recentlyViewedItemService.getRecentlyViewedItems(userId).pipe(
      take(1),
      takeUntil(this._destroy$)
    )
    .subscribe( result => {
      const data = result.data as MenuItemModel[];
      this.recentlyViewedItems = data?.length > 3 ? data : [];
    });
  }

  onAddToCart(itemId: number) {
    this.addToCart.emit(itemId);
  }

  ngOnDestroy() {
    this._destroy$.next();
  }
}
