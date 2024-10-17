import {Component, inject, Input, OnDestroy} from '@angular/core';
import {UserItemRatingService} from "../../../core/service/user-item-rating.service";
import {UserItemRatingModel} from "../../../core/model/userItemRating/user-item-rating.model";
import {DxButtonModule, DxNumberBoxModule, DxPopupModule, DxTextAreaModule} from "devextreme-angular";
import {AuthService} from "../../../core/service/auth.service";
import {take, takeUntil} from "rxjs/operators";
import {Subject} from "rxjs";
import {CommonModule, NgForOf} from "@angular/common";
import {RatingStarComponent} from "./rating-star/rating-star.component";
import {UserItemRatingSummaryModel} from "../../../core/model/userItemRating/user-rating-summary.model";

@Component({
  selector: 'sc-user-item-rating',
  standalone: true,
  imports: [
    DxTextAreaModule,
    DxButtonModule,
    DxNumberBoxModule,
    CommonModule,
    RatingStarComponent,
    DxPopupModule
  ],
  templateUrl: './user-item-rating.component.html',
  styleUrl: './user-item-rating.component.css'
})
export class UserItemRatingComponent implements  OnDestroy{

  @Input() itemId!: number;
  ratingDescription: string = '';
  ratings: UserItemRatingSummaryModel[] = [];
  userId!: number;
  currentUserRating!: UserItemRatingModel;
  isAddRatingVisible = false;
  currentRating = 0;
  private _authService: AuthService = inject(AuthService);
  private _ratingService = inject(UserItemRatingService);
  private _destroy$ = new Subject<void>();

  ngOnInit(): void {
    this.userId = this._authService.getCurrentUserId();
    this.getUserItemRating();
    this.loadRatings();
  }

  submitRating() {
    const ratingData: UserItemRatingModel = {
      userId: this.userId,
      itemId: this.itemId,
      rating: this.currentRating,
      ratingDescription: this.ratingDescription
    };

    this._ratingService.rateItem(ratingData).subscribe(() => {
      this.isAddRatingVisible = false;
      this.loadRatings(); // Reload ratings after submission
    });
  }

  loadRatings() {
    this._ratingService.getItemRatings(this.itemId).pipe(
      take(1),
      takeUntil(this._destroy$)
    )
      .subscribe((data) => {
      const ratings = data.data as UserItemRatingSummaryModel[];
      this.resortRatings(ratings);
    });
  }

  resortRatings(ratings: UserItemRatingSummaryModel[]) {
    // Find the current user's rating
    let currentUserRatingIndex = ratings.findIndex((r) => r.userId === this.userId);

    // If the current user has a rating, move it to the top
    if (currentUserRatingIndex !== -1) {
      const currentUserRating = ratings.splice(currentUserRatingIndex, 1)[0]; // Remove the rating from the array
      ratings.unshift(currentUserRating); // Add the current user rating to the top
    }

    // Update the ratings array
    this.ratings = ratings;
  }

  getUserItemRating() {
    this._ratingService.getUserItemRating(this.userId, this.itemId).pipe(
      take(1),
      takeUntil(this._destroy$)
    )
      .subscribe(s =>{
        this.currentUserRating = s.data as UserItemRatingModel;
        this.currentRating = s.data?.rating ?? 0;
        this.ratingDescription = s.data?.ratingDescription ?? '';
      })
  }

  ngOnDestroy(): void {
   this._destroy$.next();
  }

}
