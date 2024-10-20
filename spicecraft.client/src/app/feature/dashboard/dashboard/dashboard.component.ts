import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {CommonModule} from "@angular/common";
import {DxButtonModule} from "devextreme-angular";
import {TitleComponent} from "../../../shared/components/title/title.component";
import {Subject} from "rxjs";
import {AuthService} from "../../../core/service/auth.service";
import {UserService} from "../../../core/service/user.service";
import {CustomerDashboardService} from "../../../core/service/customer-dashboard.service";
import {CustomerDashboardModel} from "../../../core/model/customer/customer-dashboard.model";
import {RecommendedItemsComponent} from "../../item/recommended-items/recommended-items.component";

@Component({
  selector: 'sc-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    DxButtonModule,
    TitleComponent,
    RecommendedItemsComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit, OnDestroy {

  public profileImage: string = '';
  public currentUserName!: string;
  public dashboardData: CustomerDashboardModel | null = null;
  public userId = 0;
  public isInternalUser = false;

  private _authService: AuthService = inject(AuthService);
  private _userService: UserService = inject(UserService);
  private _dashboardService = inject(CustomerDashboardService);
  private _destroy$: Subject<void> = new Subject<void>();

  ngOnInit(): void {
    this.userId = this._authService.getCurrentUserId();
    this.isInternalUser = this._authService.isInternalUser();
    this.currentUserName = this._userService.getUserFirstName();
    this.loadDashboardData(this.userId);
  }

  loadDashboardData(userId: number): void {
    this._dashboardService.getCustomerDashboard(userId).subscribe(
      (data: CustomerDashboardModel) => {
        this.dashboardData = data;
      },
      (error) => {
        console.error('Failed to load dashboard data', error);
      }
    );
  }

  ngOnDestroy(): void {
   this._destroy$.next();
  }
}
