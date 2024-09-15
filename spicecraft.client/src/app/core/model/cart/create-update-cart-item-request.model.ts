export interface CreateUpdateCartItemRequest {
  quantity: number;
  userId: number;
  cartId?: number;  // Defaults can be set in the logic that creates this object
  itemId: number;
  priceAtAdd?: number;  // Optional as it's nullable in C#
  description?: string;  // Defaults can be set in the logic that creates this object
  size?: string;
  spiceLevel?: string;
}
