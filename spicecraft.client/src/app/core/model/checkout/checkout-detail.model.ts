// checkout-detail.model.ts
import {ShippingOptionModel} from "../shipping/shipping-option-model";

export interface CheckoutDetailModel {
  userAddress: string;
  subTotal: number;
  toPay: number;
  shippingOptions: ShippingOptionModel[];
  qualifiedForFreeShipping: boolean;
}
