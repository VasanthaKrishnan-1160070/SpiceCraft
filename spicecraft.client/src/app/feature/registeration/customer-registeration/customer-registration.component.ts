import { Component } from '@angular/core';
import {CommonModule} from "@angular/common";
import {DxButtonModule, DxFormModule} from "devextreme-angular";
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-customer-registration',
  templateUrl: './customer-registration.component.html',
  styleUrl: './customer-registration.component.css',
  standalone: true,
  imports: [
    CommonModule,
    DxButtonModule,
    DxFormModule,
    TitleComponent,
  ]
})
export class CustomerRegistrationComponent {
  minDobDate = new Date(1900, 0, 1);
  maxDobDate = new Date();
  customerRegistration = {
     firstName: '',
     lastName: '',
     userName: '',
     email: '',
     phoneNumber: '',
     dateOfBirth: '',
     password: '',
     confirmPassword: '',
     streetAddress1: '',
     streetAddress2: '',
     city: '',
     state: '',
     postalCode: ''
  }

  confirmPasswordComparison = () => {
    return this.customerRegistration.password;
  }

  onRegister(event: any): void {

  }

}
