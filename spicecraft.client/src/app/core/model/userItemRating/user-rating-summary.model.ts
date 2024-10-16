export interface UserItemRatingSummaryModel {
  userItemRatingId?: number; // Optional field, equivalent to int? in C#
  userId: number;
  userName: string;
  firstName: string;
  rating: number;
  ratingDescription: string;
  createdAt?: Date;  // Optional field, equivalent to DateTime? in C#
  updatedAt?: Date;  // Optional field, equivalent to DateTime? in C#
}
