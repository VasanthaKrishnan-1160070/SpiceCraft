import {Component, inject, OnDestroy} from '@angular/core';
import {UserService} from "../../core/service/user.service";
import {DxButtonModule, DxFormModule} from "devextreme-angular";
import {AuthService} from "../../core/service/auth.service";
import {NgIf} from "@angular/common";
import {Subject, takeUntil} from "rxjs";
import {Router} from "@angular/router";

@Component({
  selector: 'sc-login',
  standalone: true,
  imports: [
    DxFormModule,
    DxButtonModule,
    NgIf
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements  OnDestroy {
  authService = inject(AuthService);
  router = inject(Router);
  loginSuccess?: boolean;
  userActive = true;
  loginMessage = '';
  $destroy: Subject<void> = new Subject<void>();
  loginCredential = {
    userName: '',
    password: ''
  }

  login() {
    const resp = this.authService
       .login(this.loginCredential.userName, this.loginCredential.password)
       .pipe(
         takeUntil(this.$destroy),
    )
    .subscribe(
      status => {
        if (status) {
          const isUserActive = this.authService.isUserActive();

          if (isUserActive) {
            this.loginSuccess = status;
            this.loginMessage = `Login Successful`;
            this.userActive = isUserActive;
            this.router.navigate(['dashboard']);
          }
          else {
            this.loginSuccess = status;
            this.userActive = isUserActive;
            this.loginMessage = `Login Successful, but your account is inactive. Please contact us.`;
          }
        }
        else {
          this.loginSuccess = false;
          this.loginMessage = `Login Failed, Please enter valid user name and password`;
        }
      }
    )
  }

  ngOnDestroy() {
    this.$destroy.next();
  }
}
