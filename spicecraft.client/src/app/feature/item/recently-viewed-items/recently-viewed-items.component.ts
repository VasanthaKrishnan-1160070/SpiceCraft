import {Component, OnDestroy, OnInit} from '@angular/core';
import {RecentlyViewedItemModel} from "../../../core/model/recentlyViewed/RecentlyViewedItemModel";
import {RecentlyViewedItemService} from "../../../core/service/recently-viewed-item.service";
import {Subject} from "rxjs";
import {take, takeUntil} from "rxjs/operators";

@Component({
  selector: 'sc-recently-viewed-items',
  standalone: true,
  imports: [],
  templateUrl: './recently-viewed-items.component.html',
  styleUrl: './recently-viewed-items.component.css'
})
export class RecentlyViewedItemsComponent implements OnInit, OnDestroy {
  recentlyViewedItems: RecentlyViewedItemModel[] = [];
  userId: number = 1; // Set to the currently logged-in user
  private _destroy$: Subject<void> = new Subject<void>();

  constructor(private recentlyViewedItemService: RecentlyViewedItemService) {}

  ngOnInit(): void {
    this.loadRecentlyViewedItems();
  }

  loadRecentlyViewedItems(): void {
    this.recentlyViewedItemService.getRecentlyViewedItems(this.userId).pipe(
      take(1),
      takeUntil(this._destroy$)
    )
        .subscribe( result =>
      this.recentlyViewedItems = result.data as RecentlyViewedItemModel[]
    );
  }

  ngOnDestroy() {
    this._destroy$.next();
  }
}
