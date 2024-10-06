import {Component, inject, Input, OnInit, ViewChild} from '@angular/core';
import {TitleComponent} from "../../shared/components/title/title.component";
import {DxFormComponent, DxFormModule} from "devextreme-angular";
import {DxiItemModule, DxiValidationRuleModule} from "devextreme-angular/ui/nested";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {CommonModule, NgIf} from "@angular/common";
import {UserService} from "../../core/service/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {RegisterModel} from "../../core/interface/register-user.interface";
import {UserModel} from "../../core/model/user/user.model";
import {takeUntil} from "rxjs/operators";
import {Subject} from "rxjs";
import {NotifyService} from "../../core/service/notify.service";

@Component({
  selector: 'sc-profile',
  standalone: true,
    imports: [
        TitleComponent,
        DxFormModule,
        DxiItemModule,
        DxiValidationRuleModule,
        FormsModule,
        NgIf,
        ReactiveFormsModule,
        CommonModule
    ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  @Input() userId!: number;
  @Input() canEdit: boolean = true;
  @ViewChild(DxFormComponent, { static: false }) form!: DxFormComponent;
  isRegisterSuccess?: boolean;
  _userService = inject(UserService);
  router = inject(Router);
  private _notifyService = inject(NotifyService);
  private _destroy$: Subject<void> = new Subject<void>();
  activatedRoute = inject(ActivatedRoute);
  minDobDate = new Date(1900, 0, 1);
  maxDobDate = new Date();
  public userModel!: UserModel | null | undefined;

  ngOnInit() {
    this._userService.getUserDetailsById(this.userId).pipe(
      takeUntil(this._destroy$)
    ).subscribe(
      s => this.userModel = s.data
    );
  }

  usernameAsyncValidation = (params: any) => {
    return new Promise((resolve, reject) => {
      this._userService.checkUserName(params.value, this.userId).subscribe(isValid => {
        isValid ? resolve(isValid) : reject(isValid);
      }, () => {
        // Handle any errors by resolving with an invalid state
        resolve({ isExists: true });
      });
    });
  };

  emailAsyncValidation = (params: any) => {
    return new Promise((resolve, reject) => {
      this._userService.checkEmail(params.value, this.userId).subscribe(isValid => {
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
      (this.userModel as UserModel).title = 'Mr';
      this._userService.createOrUpdate(this.userModel as UserModel)
        .subscribe(status => {
          if (status.isSuccess) {
            this.isRegisterSuccess = true;
            this._notifyService.showSuccess('Profile Updated Successfully');
          }
          else {
            this.isRegisterSuccess = false;
            this._notifyService.showSuccess('Could not Update the profile');
          }
        })
    }
  }
}
