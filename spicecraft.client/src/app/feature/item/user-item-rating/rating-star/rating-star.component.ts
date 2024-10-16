import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NgClass} from "@angular/common";

@Component({
  selector: 'sc-rating-star',
  standalone: true,
  imports: [
    NgClass
  ],
  templateUrl: './rating-star.component.html',
  styleUrl: './rating-star.component.css'
})
export class RatingStarComponent {
  @Input() currentRating: number = 0;
  @Input() editable: boolean = false;
  @Output() currentRatingChange = new EventEmitter<number>();

  ratingClick(item: number, isFirstItem: boolean) {
    if (!this.editable) {
      return;
    }
    if (isFirstItem && this.currentRating === 1) {
      this.currentRating = 0;
      this.currentRatingChange.emit(this.currentRating);
      return;
    }
    this.currentRating = item;
    this.currentRatingChange.emit(this.currentRating);
  }

}
