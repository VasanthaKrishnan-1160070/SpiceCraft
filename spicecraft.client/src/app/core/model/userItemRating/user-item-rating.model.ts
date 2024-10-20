export interface UserItemRatingModel {
  userItemRatingId?: number;
  userId: number;
  itemId: number;
  rating: number;
  ratingDescription: string;
  improvementDescription?: string;
  createdAt?: Date;
  updatedAt?: Date;
}
