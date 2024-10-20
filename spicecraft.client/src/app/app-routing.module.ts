import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LandingPageComponent} from "./feature/home/landing-page/landing-page.component";
import {LoginComponent} from "./feature/login/login.component";
import {ClientHomeComponent} from "./feature/home/client-home/client-home.component";
import {
  CustomerRegistrationComponent
} from "./feature/registeration/customer-registeration/customer-registration.component";
import {StaffRegistrationComponent} from "./feature/registeration/staff-registration/staff-registration.component";
import {DashboardComponent} from "./feature/dashboard/dashboard/dashboard.component";
import {LogoutComponent} from "./feature/logout/logout.component";
import {ProfileComponent} from "./feature/profile/profile.component";
import {EnquiryComponent} from "./feature/enquiry/enquiry.component";
import {ReturnsComponent} from "./feature/returns/returns.component";
import {GiftCardComponent} from "./feature/gift-card/gift-card.component";
import {B2bDashboardComponent} from "./feature/admin/b2b-dashboard/b2b-dashboard.component";
import {B2bProfileComponent} from "./feature/admin/b2b-profile/b2b-profile.component";
import {B2bDealersComponent} from "./feature/admin/b2b-dealers/b2b-dealers.component";
import {B2bStaffComponent} from "./feature/admin/b2b-staff/b2b-staff.component";
import {B2bPaymentComponent} from "./feature/admin/b2b-payment/b2b-payment.component";
import {B2bEnquiryComponent} from "./feature/admin/b2b-enquiry/b2b-enquiry.component";
import {B2bReportComponent} from "./feature/admin/b2b-report/b2b-report.component";
import {B2bShippingComponent} from "./feature/admin/b2b-shipping/b2b-shipping.component";
import {B2bOrderComponent} from "./feature/admin/b2b-order/b2b-order.component";
import {B2bInventoryComponent} from "./feature/admin/b2b-inventory/b2b-inventory.component";
import {B2bResetPasswordComponent} from "./feature/admin/b2b-reset-password/b2b-reset-password.component";
import {ItemListComponent} from "./feature/item/item-list/item-list.component";
import {B2bStaffListComponent} from "./feature/admin/b2b-staff/b2b-staff-list/b2b-staff-list.component";
import {B2bCustomerComponent} from "./feature/admin/b2b-customer/b2b-customer.component";
import {B2bCustomerListComponent} from "./feature/admin/b2b-customer/b2b-customer-list/b2b-customer-list.component";
import {EnquiryListComponent} from "./feature/enquiry/enquiry-list/enquiry-list.component";
import {EnquiryDetailComponent} from "./feature/enquiry/enquiry-detail/enquiry-detail.component";
import {CartListComponent} from "./feature/cart/cart-list/cart-list.component";
import {B2bInventoryListComponent} from "./feature/admin/b2b-inventory/b2b-inventory-list/b2b-inventory-list.component";
import {B2bPromotionComponent} from "./feature/admin/b2b-promotion/b2b-promotion/b2b-promotion.component";
import {CustomerCheckoutComponent} from "./feature/checkout/customer-checkout/customer-checkout.component";
import {BulkCustomerComponent} from "./feature/checkout/bulk-customer/bulk-customer.component";
import {OrderListComponent} from "./feature/order/order-list/order-list.component";
import {OrderDetailComponent} from "./feature/order/order-detail/order-detail.component";
import {PaymentListComponent} from "./feature/payment/payment-list/payment-list.component";
import {PaymentDetailComponent} from "./feature/payment/payment-detail/payment-detail.component";
import {MyProfileComponent} from "./feature/profile/my-profile/my-profile.component";
import {AddProfileComponent} from "./feature/profile/add-profile/add-profile.component";
import {UserProfileComponent} from "./feature/profile/user-profile/user-profile.component";
import {ItemDetailComponent} from "./feature/item/item-detail/item-detail.component";

const routes: Routes = [
  { path: '', component: ClientHomeComponent},
  { path: 'home', component: ClientHomeComponent},
  { path: 'login', component: LoginComponent },
  { path: 'customer-registration', component: CustomerRegistrationComponent },
  { path: 'staff-registration', component: StaffRegistrationComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'profile', component: MyProfileComponent },
  { path: 'item-list', component: ItemListComponent },
  { path: 'item-detail/:itemId', component: ItemDetailComponent },
  { path: 'cart-list', component: CartListComponent },
  { path: 'order-list', component: OrderListComponent },
  { path: 'order-detail/:orderId', component: OrderDetailComponent },
  { path: 'payment-list', component: PaymentListComponent },
  { path: 'payment-detail/:transactionId', component: PaymentDetailComponent },
  { path: 'enquiry-list', component: EnquiryListComponent },
  { path: 'enquiry-details/:id', component: EnquiryDetailComponent },
  { path: 'returns', component: ReturnsComponent },
  { path: 'gift-card', component: GiftCardComponent },
  { path: 'customer-checkout', component: CustomerCheckoutComponent },
  { path: 'bulk-customer-checkout', component: BulkCustomerComponent },
  { path: 'logout', component: LogoutComponent },

  // b2b routes
  { path: 'b2b-dashboard', component: B2bDashboardComponent },
  { path: 'b2b-profile', component: B2bProfileComponent },
  { path: 'b2b-dealer-list', component: B2bDealersComponent },
  { path: 'b2b-staff-list', component: B2bStaffListComponent },
  { path: 'b2b-customer-list', component: B2bCustomerListComponent },
  { path: 'b2b-staff-detail', component: B2bStaffComponent },
  { path: 'b2b-payment-list', component: B2bPaymentComponent },
  { path: 'b2b-enquiry-list', component: B2bEnquiryComponent },
  { path: 'b2b-report', component: B2bReportComponent },
  { path: 'b2b-shipping', component: B2bShippingComponent },
  { path: 'b2b-order-list', component: B2bOrderComponent },
  { path: 'b2b-inventory', component: B2bInventoryComponent },
  { path: 'b2b-promotion-list', component: B2bPromotionComponent},
  { path: 'b2b-reset-password', component: B2bResetPasswordComponent },
  { path: 'create-staff/:roleId/:title', component: AddProfileComponent},
  { path: 'create-customer/:roleId/:title', component: AddProfileComponent},
  { path: 'create-manager/:roleId/:title', component: AddProfileComponent},
  { path: 'create-dealer/:roleId/:title', component: AddProfileComponent},
  { path: 'view-user/:userId/:title', component: UserProfileComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
