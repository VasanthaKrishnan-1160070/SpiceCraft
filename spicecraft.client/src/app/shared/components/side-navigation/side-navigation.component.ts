import {Component, input, computed} from '@angular/core';

@Component({
  selector: 'sc-side-navigation',
  standalone: true,
  imports: [],
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
       {name: 'Home', icon: ''},
       {name: 'Dashboard', icon: ''},
       {name: 'Profile', icon: ''},
       {name: 'Orders', icon: ''},
       {name: 'Payments', icon: ''},
       {name: 'Enquiry', icon: ''},
       {name: 'Returns', icon: ''},
       {name: 'Gift Card', icon: ''},
       {name: 'Logout', icon: ''},
     ];
    }
    else {
      return [
        {name: 'Home', icon: ''},
        {name: 'Dashboard', icon: ''},
        {name: 'Profile', icon: ''},
        {name: 'Orders', icon: ''},
        {name: 'Payments', icon: ''},
        {name: 'Enquiry', icon: ''},
        {name: 'Returns', icon: ''},
        {name: 'Gift Card', icon: ''},
        {name: 'Logout', icon: ''},
      ];
    }
  }


}
