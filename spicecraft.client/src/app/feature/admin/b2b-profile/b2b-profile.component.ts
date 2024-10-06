import {Component, inject, ViewChild} from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {DxFormComponent, DxFormModule} from "devextreme-angular";
import {DxiItemModule, DxiValidationRuleModule} from "devextreme-angular/ui/nested";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {NgIf} from "@angular/common";
import {UserService} from "../../../core/service/user.service";
import {Router} from "@angular/router";
import {RegisterModel} from "../../../core/interface/register-user.interface";
import {MyProfileComponent} from "../../profile/my-profile/my-profile.component";

@Component({
  selector: 'sc-b2b-profile',
  templateUrl: './b2b-profile.component.html',
  styleUrl: './b2b-profile.component.css',
  standalone: true,
  imports: [
    TitleComponent,
    DxFormModule,
    DxiItemModule,
    DxiValidationRuleModule,
    FormsModule,
    NgIf,
    ReactiveFormsModule,
    MyProfileComponent
  ]
})
export class B2bProfileComponent {
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
    password: '!Apple123',
    confirmPassword: '!Apple123',
    streetAddress1: '',
    streetAddress2: '',
    city: '',
    state: '',
    postalCode: '',
    roleId: 3
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
      this._userService.registerStaff(this.customerRegistration)
        .subscribe(status => {
          if (status.success) {
            this.isRegisterSuccess = true;
          }
          else {
            this.isRegisterSuccess = false;
          }
        })
    }
  }
}
