import {inject, Injectable} from '@angular/core';
import {WebAPIService} from "./webAPI.service";
import { UserItemRatingModel} from "../model/userItemRating/user-item-rating.model";
import {Observable} from "rxjs";
import {ResultDetailModel} from "../model/result-detail.model";
import {UserItemRatingSummaryModel} from "../model/userItemRating/user-rating-summary.model";
import { StarRatingSummaryModel } from '../model/userItemRating/star-rating-summary.model';

@Injectable({
  providedIn: 'root'
})
export class UserItemRatingService {
  private _api = inject(WebAPIService);


  // Submit or update rating
  rateItem(rating: UserItemRatingModel): Observable<ResultDetailModel<UserItemRatingModel>> {
    return this._api.post<ResultDetailModel<UserItemRatingModel>>(`/rating/rate-item`, rating);
  }

  // Get ratings for an item
  getItemRatings(itemId: number): Observable<ResultDetailModel<UserItemRatingSummaryModel[]>> {
    return this._api.get<ResultDetailModel<UserItemRatingSummaryModel[]>>(`/rating/get-item-ratings/${itemId}`);
  }

  // Get a user's rating for an item
  getUserItemRating(userId: number, itemId: number): Observable<ResultDetailModel<UserItemRatingModel>> {
    return this._api.get<ResultDetailModel<UserItemRatingModel>>(`/rating/get-user-rating/${userId}/${itemId}`);
  }

  getStarRatingsSummary(itemId: number): Observable<ResultDetailModel<StarRatingSummaryModel[]>> {
    return this._api.get<ResultDetailModel<StarRatingSummaryModel[]>>(`/rating/ratings/${itemId}`);
  }
}

