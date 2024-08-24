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
        this.loginSuccess = status;
        if (this.loginSuccess) {
          this.router.navigate(['dashboard']);
        }
      }
    )
  }

  ngOnDestroy() {
    this.$destroy.next();
  }
}
