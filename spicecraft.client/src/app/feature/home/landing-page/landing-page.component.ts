import {AfterViewInit, Component, CUSTOM_ELEMENTS_SCHEMA, inject, OnDestroy, OnInit} from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import * as AOS from 'aos';
import { UserService } from '../../../core/service/user.service';
import {ActivatedRoute, Router, RouterLink, RouterLinkActive, RouterOutlet, NavigationEnd } from "@angular/router";
import {CommonModule} from "@angular/common";
import {filter, takeUntil} from 'rxjs/operators';
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";
import {Subject} from "rxjs";
import {SideNavigationComponent} from "../../../shared/components/side-navigation/side-navigation.component";
import {AuthService} from "../../../core/service/auth.service";

@Component({
  selector: 'sc-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css',
  standalone: true,
  imports: [
    CommonModule,
    TitleComponent,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    SideNavigationComponent
  ]
})
export class LandingPageComponent implements OnInit, OnDestroy {
  currentRoute: string = '';
  router: Router = inject(Router);
  activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  destroy$: Subject<void> = new Subject();
  isIntroPage: boolean = true;
  isAuthenticated: boolean = false;
  authService: AuthService = inject(AuthService);

  ngOnInit(): void {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.currentRoute = this.activatedRoute.snapshot.firstChild?.routeConfig?.path || '';
      this.checkIsIntroPage(this.currentRoute);
      this.isAuthenticated = this.authService.isAuthenticated();
    });
  }

  checkIsIntroPage(currentRoute: string): void {
    this.isIntroPage = currentRoute === 'home' || currentRoute === 'login' || currentRoute === 'customer-registration' || currentRoute === 'staff-registration';
  }

  ngOnDestroy() {
    this.destroy$.next();
  }

}
