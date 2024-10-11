import {OrderDetailModel} from "../order/order-detail-model";
import {UserAddressModel} from "../user/user-address.model";
import {ContactInfoModel} from "../user/contact-info.model";

export interface PaymentInvoiceModel {
  userId: number;
  orderId: number;
  transactionId: number;
  amount: number;
  paymentDate: string;  // Date formatted as 'dd/MM/yyyy'
  paymentMethod: string;
  paymentStatus: string;
  shippingOptionName: string;
  countryName: string;
  isFreeShipping: boolean;
  shippingCost: number;
  subTotal: number;
  isPickUp: boolean;
  orderDetails: OrderDetailModel[];  // List of order details
  userAddress: UserAddressModel;
  contactInfo: ContactInfoModel;
}
