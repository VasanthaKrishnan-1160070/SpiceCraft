import {UserAddressModel} from "../user/user-address.model";
import {ContactInfoModel} from "../user/contact-info.model";
import {OrderDetailModel} from "./order-detail-model";

export interface UserOrderDetailModel {
  orderId: number;
  userId: number;
  orderDate: string;
  shippingAddress: UserAddressModel;
  contactInfo: ContactInfoModel;
  orderStatus: string;
  totalCost: number;
  orderItems: OrderDetailModel[];
  shippingOptionId: number;
}
