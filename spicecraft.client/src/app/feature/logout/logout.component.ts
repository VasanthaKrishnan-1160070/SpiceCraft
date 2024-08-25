import {Component, inject, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {AuthService} from "../../core/service/auth.service";

@Component({
  selector: 'sc-logout',
  standalone: true,
  imports: [],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css'
})
export class LogoutComponent implements  OnInit{

  router = inject(Router);
  authService = inject(AuthService);

   ngOnInit() {
     this.authService.logout();
     this.router.navigate(['home']);
   }
}
