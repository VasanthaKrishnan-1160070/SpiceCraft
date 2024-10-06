import {Component, inject, Input, ViewChild} from '@angular/core';
import {DxFormComponent, DxFormModule} from "devextreme-angular";
import {DxiItemModule, DxiValidationRuleModule} from "devextreme-angular/ui/nested";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {UserService} from "../../../core/service/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {NotifyService} from "../../../core/service/notify.service";
import {Subject} from "rxjs";
import {UserModel} from "../../../core/model/user/user.model";
import {takeUntil} from "rxjs/operators";
import {TitleComponent} from "../../../shared/components/title/title.component";
import { Location } from '@angular/common';

@Component({
  selector: 'sc-add-profile',
  standalone: true,
  imports: [
    DxFormModule,
    DxiItemModule,
    DxiValidationRuleModule,
    FormsModule,
    ReactiveFormsModule,
    TitleComponent
  ],
  templateUrl: './add-profile.component.html',
  styleUrl: './add-profile.component.css'
})
export class AddProfileComponent {
  public title: string = 'Create Profile';
  @ViewChild(DxFormComponent, { static: false }) form!: DxFormComponent;
  isRegisterSuccess?: boolean;
  _userService = inject(UserService);
  router = inject(Router);
  _locationService = inject(Location);
  private _roleId!: number;
  private _notifyService = inject(NotifyService);
  private _destroy$: Subject<void> = new Subject<void>();
  private _router: Router = inject(Router);
  private _activatedRoute = inject(ActivatedRoute);
  activatedRoute = inject(ActivatedRoute);
  minDobDate = new Date(1900, 0, 1);
  maxDobDate = new Date();
  public userModel!: UserModel | null | undefined;

  ngOnInit() {
    this._activatedRoute.paramMap.subscribe(params => {
      this._roleId = +(params.get('roleId') || 0);
      this.title = params.get('title') || 'Create Profile';
    });
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

  onSave(event: any): void {
    event.preventDefault();
    const isFormValid = this.form.instance.validate().isValid;

    if (isFormValid) {
      (this.userModel as UserModel).title = 'Mr.';
      (this.userModel as UserModel).roleId = this._roleId;
      this._userService.createOrUpdate(this.userModel as UserModel)
        .subscribe(status => {
          if (status.isSuccess) {
            this.isRegisterSuccess = true;
            this._notifyService.showSuccess('Profile Created Successfully');
            this._locationService.back();
          }
          else {
            this.isRegisterSuccess = false;
            this._notifyService.showSuccess('Could not Create the profile');
          }
        })
    }
  }
}
