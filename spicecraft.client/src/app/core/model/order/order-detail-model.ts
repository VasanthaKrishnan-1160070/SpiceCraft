export interface OrderDetailModel {
  orderId: number;
  itemId: number;   // Was ProductId
  itemName: string; // Was ProductName
  quantity: number;
  purchasePrice: number;
  price: number;
}
