import {Component, input, computed} from '@angular/core';
import {RouterLink, RouterLinkActive} from "@angular/router";

@Component({
  selector: 'sc-side-navigation',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './side-navigation.component.html',
  styleUrl: './side-navigation.component.css'
})
export class SideNavigationComponent {

  userType = input<string>('client');

  navigation = computed(() => {
    return this.getNavigation(this.userType());
  });

  getNavigation(userType: string) {
    if (userType === 'client') {
     return [
       {name: 'Home', icon: 'fa-solid fa-house', routing: '/home'},
       {name: 'Dashboard', icon: 'fa-solid fa-magnifying-glass-chart', routing: '/dashboard'},
       {name: 'Profile', icon: 'fa-regular fa-address-card', routing: '/profile'},
       {name: 'Orders', icon: 'fa-solid fa-box-open', routing: '/order'},
       {name: 'Payments', icon: 'fa-solid fa-file-invoice-dollar', routing: '/payment'},
       {name: 'Enquiry', icon: 'fa-solid fa-question', routing: '/enquiry'},
       {name: 'Returns', icon: 'fa-solid fa-arrow-rotate-left', routing: '/returns'},
       {name: 'Gift Card', icon: 'fa-solid fa-hand-holding-dollar', routing: '/gift-card'},
       {name: 'Logout', icon: 'fa-solid fa-right-from-bracket', routing: '/logout'},
     ];
    }
    else {
      return [
        {name: 'Home', icon: 'fa-solid fa-house', routing: '/home'},
        {name: 'Dashboard', icon: 'fa-solid fa-magnifying-glass-chart', routing: '/b2b-dashboard'},
        {name: 'My Profile', icon: 'fa-solid fa-hand-holding-dollar', routing: '/b2b-profile'},
        {name: 'Dealers', icon: 'fa-regular fa-address-card', routing: '/b2b-dealer'},
        {name: 'Staffs', icon: 'fa-solid fa-box-open', routing: '/b2b-staff'},
        {name: 'Customers', icon: 'fa-solid fa-file-invoice-dollar', routing: '/b2b-payment'},
        {name: 'Enquiry', icon: 'fa-solid fa-question', routing: '/b2b-enquiry'},
        {name: 'Reports', icon: 'fa-solid fa-arrow-rotate-left', routing: '/b2b-report'},
        {name: 'Payments', icon: 'fa-solid fa-hand-holding-dollar', routing: 'b2b-payment'},
       // {name: 'Shipping', icon: 'fa-solid fa-hand-holding-dollar', routing: 'b2b-shipping'},
        {name: 'Orders', icon: 'fa-solid fa-hand-holding-dollar', routing: '/b2b-order'},
        {name: 'Inventory', icon: 'fa-solid fa-hand-holding-dollar', routing: 'b2b-inventory'},
       // {name: 'Reset Password', icon: 'fa-solid fa-hand-holding-dollar', routing: 'b2b-reset-password'},
        {name: 'Logout', icon: 'fa-solid fa-right-from-bracket', routing: '/logout'},
      ];
    }
  }


}
