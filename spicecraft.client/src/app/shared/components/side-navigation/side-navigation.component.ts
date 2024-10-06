import {Component, input, computed, Input, OnInit, inject, OnDestroy, signal} from '@angular/core';
import {NavigationEnd, Router, RouterLink, RouterLinkActive} from "@angular/router";
import {NavigationService} from "../../../core/service/navigation.service";
import {UserService} from "../../../core/service/user.service";
import {map, take, takeUntil} from "rxjs/operators";
import {Subject} from "rxjs";
import {NavigationPredictionModel} from "../../../core/model/ml/navigation/navigation-prediction.model";
import {ResultDetailModel} from "../../../core/model/result-detail.model";

@Component({
  selector: 'sc-side-navigation',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './side-navigation.component.html',
  styleUrl: './side-navigation.component.css'
})
export class SideNavigationComponent implements  OnInit, OnDestroy {
  private _navigationService: NavigationService = inject(NavigationService);
  private _router = inject(Router);
  private _userService: UserService = inject(UserService);
  private _destroy$: Subject<void> = new Subject<void>();
  private _refresh = signal<boolean>(false);

  private _startTime!: number;
  private _previousUrl!: string;
  private _clickCount = 0;

  clientNavigationArray = [
    {name: 'Home', icon: 'fa-solid fa-house', routing: '/home'},
    {name: 'Dashboard', icon: 'fa-solid fa-magnifying-glass-chart', routing: '/dashboard'},
    {name: 'Profile', icon: 'fa-regular fa-address-card', routing: '/profile'},
    {name: 'Menu', icon: 'fa-solid fa-box-open', routing: '/item-list'},
    {name: 'Orders', icon: 'fa-solid fa-box-open', routing: '/order-list'},
    {name: 'Cart', icon: 'fa-solid fa-box-open', routing: '/cart-list'},
    {name: 'Payments', icon: 'fa-solid fa-file-invoice-dollar', routing: '/payment-list'},
    {name: 'Enquiry', icon: 'fa-solid fa-question', routing: '/enquiry-list'},
   // {name: 'Returns', icon: 'fa-solid fa-arrow-rotate-left', routing: '/returns'},
    {name: 'Gift Card', icon: 'fa-solid fa-hand-holding-dollar', routing: '/gift-card'},
    {name: 'Logout', icon: 'fa-solid fa-right-from-bracket', routing: '/logout'},
  ];

  userType = input<string>('client');
  firstName = input<string>('');
  predictions!: any[];

  navigation = computed(() => {
    if (this._refresh()) {
      return this.getNavigation(this.userType());
    }
    else {
      return this.getNavigation(this.userType());
    }
  });

  ngOnInit(): void {
    if (this.userType() === 'client') {
      const userId = this._userService.getCurrentUserId();
      this.listenForNavigationEvent(userId);
      // this.getNavigationPredictions(userId);
    }
  }

  private listenForNavigationEvent(userId: number) {
    this._router
      .events
      .pipe(takeUntil(this._destroy$))
      .subscribe(event => {
        if (event instanceof NavigationEnd) {
          const timeSpent = Date.now() - this._startTime;
          const navigationItem = this.clientNavigationArray.filter(f => f.routing === event.url)[0].name;
          if (this._previousUrl) {
            // Log the previous navigation item
            this._navigationService
              .logNavigationActivity(userId, navigationItem, event.url, timeSpent / 1000, this._clickCount)
              .pipe(takeUntil(this._destroy$))
              .subscribe();
          }

          // Reset for the new page
          this._startTime = Date.now();
          this._previousUrl = event.urlAfterRedirects;
          this._clickCount = 0;
        }
      })
  }

  onNavigationItemClick(navigationItem: string): void {
    if (this.userType() === 'client') {
      this._clickCount++;
      // this.logService.logNavigationActivity('user-id-123', navigationItem, 0, this.clickCount).subscribe();
    }
  }

  getNavigationPredictions(userId: number) {
    this._navigationService
      .getNavigationPredictions(userId)
      .pipe(
        map((predictionsData: ResultDetailModel<NavigationPredictionModel[]>) => {
          if (!predictionsData?.data || predictionsData.data.length === 0) {
            return this.clientNavigationArray; // Return default navigation if no predictions
          }

          const predictions = predictionsData.data as NavigationPredictionModel[];

          // Sort the navigation array based on predictions (predicted items at the top)
          const sortedNavigation = this.clientNavigationArray.sort((a, b) => {
            // Find the predictions for the items, if they exist
            const predictionA = predictions.find(p => p.name === a.name)?.likelihood ?? -Infinity;
            const predictionB = predictions.find(p => p.name === b.name)?.likelihood ?? -Infinity;

            return predictionB - predictionA; // Sort descending by likelihood
          });

          return sortedNavigation;
        }),
        takeUntil(this._destroy$)
      )
      .subscribe(
        (data: any) => {
          if (data) {
            this.predictions = data;
            this._refresh.set(true);
          }
        },
        error => {
          console.error('Error fetching predictions:', error);
        }
      );
  }

  getNavigation(userType: string) {
    if (userType === 'client') {
      if (this.predictions && this.predictions.length > 0) {
        // Sort the navigation array based on predictions (you can sort by Probability or Likelihood)
       return this.predictions;
      }
     return this.clientNavigationArray
    }
    else {
      return [
        {name: 'Home', icon: 'fa-solid fa-house', routing: '/home'},
        {name: 'Dashboard', icon: 'fa-solid fa-magnifying-glass-chart', routing: '/b2b-dashboard'},
        {name: 'My Profile', icon: 'fa-solid fa-hand-holding-dollar', routing: '/b2b-profile'},
        {name: 'Menu', icon: 'fa-solid fa-box-open', routing: '/item-list'},
        {name: 'Dealers', icon: 'fa-regular fa-address-card', routing: '/b2b-dealer-list'},
        {name: 'Staffs', icon: 'fa-solid fa-box-open', routing: '/b2b-staff-list'},
        {name: 'Customers', icon: 'fa-solid fa-file-invoice-dollar', routing: '/b2b-customer-list'},
        {name: 'Enquiry', icon: 'fa-solid fa-question', routing: '/b2b-enquiry-list'},
        {name: 'Reports', icon: 'fa-solid fa-arrow-rotate-left', routing: '/b2b-report'},
        {name: 'Payments', icon: 'fa-solid fa-hand-holding-dollar', routing: 'b2b-payment-list'},
        {name: 'Promotion', icon: 'fa-solid fa-hand-holding-dollar', routing: 'b2b-promotion-list'},
       // {name: 'Shipping', icon: 'fa-solid fa-hand-holding-dollar', routing: 'b2b-shipping'},
        {name: 'Orders', icon: 'fa-solid fa-hand-holding-dollar', routing: '/b2b-order-list'},
        {name: 'Inventory', icon: 'fa-solid fa-hand-holding-dollar', routing: 'b2b-inventory'},
       // {name: 'Reset Password', icon: 'fa-solid fa-hand-holding-dollar', routing: 'b2b-reset-password'},
        {name: 'Logout', icon: 'fa-solid fa-right-from-bracket', routing: '/logout'},
      ];
    }
  }

  public ngOnDestroy() {
    this._destroy$.next();
  }

}
