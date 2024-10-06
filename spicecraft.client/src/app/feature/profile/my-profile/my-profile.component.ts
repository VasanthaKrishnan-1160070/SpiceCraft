import {Component, inject, OnInit} from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {ProfileComponent} from "../profile.component";
import {UserService} from "../../../core/service/user.service";

@Component({
  selector: 'sc-my-profile',
  standalone: true,
  imports: [
    TitleComponent,
    ProfileComponent
  ],
  templateUrl: './my-profile.component.html',
  styleUrl: './my-profile.component.css'
})
export class MyProfileComponent implements OnInit {
  public currentUserId!: number;
  private _userService = inject(UserService);

  ngOnInit() {
    this.currentUserId = this._userService.getCurrentUserId();
  }
}
