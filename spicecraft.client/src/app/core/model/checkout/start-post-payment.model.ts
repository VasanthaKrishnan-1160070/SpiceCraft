// start-post-payment.model.ts
export interface StartPostPaymentModel {
  userId: number;
  shippingOptionId?: number;
  paymentMethod: string;
  giftCardCode?: string;
}
