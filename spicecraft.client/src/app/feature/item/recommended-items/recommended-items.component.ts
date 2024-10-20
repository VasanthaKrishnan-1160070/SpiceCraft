import {Component, EventEmitter, inject, OnDestroy, OnInit, Output} from '@angular/core';
import {CarouselComponent} from "../../../shared/components/carousel/carousel.component";
import {MenuItemModel} from "../../../core/model/item/menu-item.model";
import {UserService} from "../../../core/service/user.service";
import {Subject} from "rxjs";
import {RecentlyViewedItemService} from "../../../core/service/recently-viewed-item.service";
import {take, takeUntil} from "rxjs/operators";
import {RecommendationService} from "../../../core/service/recommendation.service";
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-recommended-items',
  standalone: true,
  imports: [
    CarouselComponent,
    TitleComponent
  ],
  templateUrl: './recommended-items.component.html',
  styleUrl: './recommended-items.component.css'
})
export class RecommendedItemsComponent implements OnInit, OnDestroy {
  @Output() addToCart = new EventEmitter<number>();
  recommendedItems: MenuItemModel[] = [];
  private _userService: UserService = inject(UserService);
  userId: number = 0; // Set to the currently logged-in user
  private _destroy$: Subject<void> = new Subject<void>();

  constructor(private _recommendedService: RecommendationService) {}

  ngOnInit(): void {
    this.userId = this._userService.getCurrentUserId();
    this.loadRecentlyViewedItems( this.userId );
  }

  loadRecentlyViewedItems(userId: number): void {
    this._recommendedService.getRecommendedItems(userId, 5).pipe(
      take(1),
      takeUntil(this._destroy$)
    )
      .subscribe( result => {
        const data = result as MenuItemModel[];
        this.recommendedItems = data?.length > 3 ? data : [];
      });
  }

  onAddToCart(itemId: number) {
    this.addToCart.emit(itemId);
  }

  ngOnDestroy() {
    this._destroy$.next();
  }
}

