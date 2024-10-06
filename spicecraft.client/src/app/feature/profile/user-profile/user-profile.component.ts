import {Component, inject, OnInit} from '@angular/core';
import {ProfileComponent} from "../profile.component";
import {TitleComponent} from "../../../shared/components/title/title.component";
import {UserService} from "../../../core/service/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthService} from "../../../core/service/auth.service";

@Component({
  selector: 'sc-user-profile',
  standalone: true,
    imports: [
        ProfileComponent,
        TitleComponent
    ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit {
  public routeUserId!: number;
  public canEditOtherUser: boolean = false;
  private _userService = inject(UserService);
  private _authService: AuthService = inject(AuthService);
  private _router = inject(ActivatedRoute);

  ngOnInit() {
    this._router.paramMap.subscribe(params => {
     this.routeUserId = +(params.get('userId') || 0);
    });

    this.canEditOtherUser = this._authService.isUserAdminOrManager();
  }

}
