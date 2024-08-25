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
import {OrderComponent} from "./feature/order/order.component";
import {PaymentComponent} from "./feature/payment/payment.component";
import {EnquiryComponent} from "./feature/enquiry/enquiry.component";
import {ReturnsComponent} from "./feature/returns/returns.component";
import {GiftCardComponent} from "./feature/gift-card/gift-card.component";

const routes: Routes = [
  { path: '', component: ClientHomeComponent},
  { path: 'home', component: ClientHomeComponent},
  { path: 'login', component: LoginComponent },
  { path: 'customer-registration', component: CustomerRegistrationComponent },
  { path: 'staff-registration', component: StaffRegistrationComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'orders', component: OrderComponent },
  { path: 'payment', component: PaymentComponent },
  { path: 'enquiry', component: EnquiryComponent },
  { path: 'returns', component: ReturnsComponent },
  { path: 'gift-card', component: GiftCardComponent },
  { path: 'logout', component: LogoutComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
