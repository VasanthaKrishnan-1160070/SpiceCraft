export interface UserItemRatingModel {
  userItemRatingId?: number;
  userId: number;
  itemId: number;
  rating: number;
  ratingDescription: string;
  createdAt?: Date;
  updatedAt?: Date;
}
