export interface CartItemModel {
  itemName?: string;
  unitPrice: string;
  quantity: number;
  actualPrice: string;
  discountRate: string;
  finalPrice: string;
  size?: string;
  spiceLevel?: string;
  cartItemId: number;
  cartId: number;
  itemId: number;

  priceAtAdd?: number;  // Optional field as it's nullable in C#
}
