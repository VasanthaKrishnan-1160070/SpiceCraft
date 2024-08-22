import {Component, inject} from '@angular/core';
import {UserService} from "../../core/service/user.service";
import {DxButtonModule, DxFormModule} from "devextreme-angular";
import {AuthService} from "../../core/service/auth.service";
import {NgIf} from "@angular/common";

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
export class LoginComponent {
  userService = inject(UserService);
  authService = inject(AuthService);
  loginSuccess?: boolean;
  loginCredential = {
    userName: '',
    password: ''
  }


  login() {
    const resp = this.authService.login(this.loginCredential.userName, this.loginCredential.password).subscribe(
      status => this.loginSuccess = status
    )
  }
}
