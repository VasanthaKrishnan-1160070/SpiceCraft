import {Component, inject, ViewChild} from '@angular/core';
import {CommonModule} from "@angular/common";
import {DxButtonModule, DxFormComponent, DxFormModule} from "devextreme-angular";
import {TitleComponent} from "../../../shared/components/title/title.component";
import {UserService} from "../../../core/service/user.service";
import {RegisterModel} from "../../../core/interface/register-user.interface";
import {register} from "swiper/element";
import {FormsModule} from "@angular/forms";
import {Router} from "@angular/router";

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
    FormsModule
  ]
})
export class CustomerRegistrationComponent {
  @ViewChild(DxFormComponent, { static: false }) form!: DxFormComponent;
   isRegisterSuccess?: boolean;
  _userService = inject(UserService);
  router = inject(Router);
  minDobDate = new Date(1900, 0, 1);
  maxDobDate = new Date();
  customerRegistration: RegisterModel = {
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    phoneNumber: '',
    dateOfBirth: undefined,
    password: '',
    confirmPassword: '',
    streetAddress1: '',
    streetAddress2: '',
    city: '',
    state: '',
    postalCode: '',
    roleId: 4
  }

  confirmPasswordComparison = () => {
    return this.customerRegistration.password;
  }

  usernameAsyncValidation = (params: any) => {
    return new Promise((resolve, reject) => {
      this._userService.checkUserName(params.value).subscribe(isValid => {
        isValid ? resolve(isValid) : reject(isValid);
      }, () => {
        // Handle any errors by resolving with an invalid state
        resolve({ isExists: true });
      });
    });
  };

  emailAsyncValidation = (params: any) => {
    return new Promise((resolve, reject) => {
      this._userService.checkEmail(params.value).subscribe(isValid => {
        isValid ? resolve(isValid) : reject(isValid);
      }, () => {
        // Handle any errors by resolving with an invalid state
        resolve({ isExists: true });
      });
    });
  };

  onRegister(event: any): void {
    event.preventDefault();
    const isFormValid = this.form.instance.validate().isValid;

    if (isFormValid) {
      this._userService.registerCustomer(this.customerRegistration)
        .subscribe(status => {
          if (status.success) {
            this.isRegisterSuccess = true;
            this.router.navigate(['login']);
          }
          else {
            this.isRegisterSuccess = false;
          }
        })
    }
  }

  protected readonly register = register;
}
