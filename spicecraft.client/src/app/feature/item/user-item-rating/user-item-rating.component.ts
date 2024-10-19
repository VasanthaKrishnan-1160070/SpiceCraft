import {Component, inject, Input, OnDestroy} from '@angular/core';
import {UserItemRatingService} from "../../../core/service/user-item-rating.service";
import {UserItemRatingModel} from "../../../core/model/userItemRating/user-item-rating.model";
import {
  DxButtonModule,
  DxNumberBoxModule,
  DxPopupModule,
  DxProgressBarModule,
  DxTextAreaModule
} from "devextreme-angular";
import {AuthService} from "../../../core/service/auth.service";
import {map, take, takeUntil} from "rxjs/operators";
import {Subject} from "rxjs";
import {CommonModule, NgForOf} from "@angular/common";
import {RatingStarComponent} from "./rating-star/rating-star.component";
import {UserItemRatingSummaryModel} from "../../../core/model/userItemRating/user-rating-summary.model";
import {TitleComponent} from "../../../shared/components/title/title.component";
import {StarRatingSummaryModel} from "../../../core/model/userItemRating/star-rating-summary.model";

@Component({
  selector: 'sc-user-item-rating',
  standalone: true,
  imports: [
    DxTextAreaModule,
    DxButtonModule,
    DxNumberBoxModule,
    CommonModule,
    RatingStarComponent,
    DxPopupModule,
    TitleComponent,
    DxProgressBarModule
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
  totalRatings: number = 0;
  avgRatings = '0.0';
  averageRatingText = '';
  isAdmin: boolean = false;
  isInternalUser: boolean = false;
  reviewUserId = 0;
  private _authService: AuthService = inject(AuthService);
  private _ratingService = inject(UserItemRatingService);
  private _destroy$ = new Subject<void>();

  ngOnInit(): void {
    this.isAdmin = this._authService.isUserAdmin();
    this.load();
  }

  load() {
    this.userId = this._authService.getCurrentUserId();
    this.isInternalUser = this._authService.isInternalUser();
    this.getUserItemRating();
    this.loadRatings();
    this.loadStarRatings();
  }

  starRatings: { stars: number; count: number; percentage: number; class: string }[] = [
    { stars: 5, count: 0, percentage: 0, class: 'bg-success' },
    { stars: 4, count: 0, percentage: 0, class: 'bg-info' },
    { stars: 3, count: 0, percentage: 0, class: 'bg-primary' },
    { stars: 2, count: 0, percentage: 0, class: 'bg-warning' },
    { stars: 1, count: 0, percentage: 0, class: 'bg-danger' }
  ];

  statusFormat = () => '';

  submitRating() {
    const ratingData: UserItemRatingModel = {
      userId: this.getUserId(),
      itemId: this.itemId,
      rating: this.currentRating,
      ratingDescription: this.ratingDescription
    };

    this._ratingService.rateItem(ratingData).subscribe(() => {
      this.isAddRatingVisible = false;
      this.reviewUserId = 0;
      this.load(); // Reload ratings after submission
    });
  }

  getUserId() {
    if (!this.isInternalUser) {
      return this.userId;
    }
    return this.reviewUserId || this.currentUserRating?.userId || this.userId;
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

  onEditReview(reviewUserId: number, ) {
    this.reviewUserId = reviewUserId;
    this.isAddRatingVisible = !this.isAddRatingVisible;
  }

  loadStarRatings(): void {
    this._ratingService.getStarRatingsSummary(this.itemId).pipe(
      take(1),
      takeUntil(this._destroy$),
    )
      .subscribe(
        (result) => {
          const ratings = result.data as StarRatingSummaryModel[];

          if (!ratings || !ratings?.length) {
            return;
          }

          // Step 1: Calculate total number of ratings
          this.totalRatings = ratings.reduce((sum, r) => sum + r.count, 0);

          // Step 2: Calculate the weighted sum of star ratings
          const weightedSum = ratings.reduce((sum, r) => sum + (r.stars * r.count), 0);

          // Step 3: Calculate the average rating
          const averageRating = this.totalRatings > 0 ? (weightedSum / this.totalRatings) : 0;

          // Step 4: Map star ratings with percentage and class
          this.starRatings = ratings.map(rating => ({
            stars: rating.stars,
            count: rating.count,
            percentage: this.totalRatings > 0 ? (rating.count / this.totalRatings) * 100 : 0,
            class: this.getRatingClass(rating.stars)
          }));

          // Step 5: Ensure totalRatings is shown as a decimal (even if whole number)
          this.totalRatings = +averageRating.toFixed(1);

          // Format the text for displaying average rating and review count
          this.averageRatingText = `Average based on ${ratings.reduce((sum, r) => sum + r.count, 0)} reviews.`;

          this.avgRatings = this.totalRatings.toFixed(1);
          },
        (error) => {
          console.error('Failed to load star ratings summary', error);
        }
      );
  }



  getRatingClass(stars: number): string {
    switch (stars) {
      case 5: return 'bg-success';
      case 4: return 'bg-info';
      case 3: return 'bg-primary';
      case 2: return 'bg-warning';
      case 1: return 'bg-danger';
      default: return '';
    }
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
