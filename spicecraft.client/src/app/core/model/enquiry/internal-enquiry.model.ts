import {EnquiryModel} from "./enquiry-model";
import {EnquiryCustomerModel} from "./enquiry-customer.model";

export interface InternalEnquiryModel {
  enquiries: EnquiryModel[];
  customers: EnquiryCustomerModel[];
}
