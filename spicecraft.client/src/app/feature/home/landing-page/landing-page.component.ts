import {AfterViewInit, Component, CUSTOM_ELEMENTS_SCHEMA, inject, OnInit} from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import * as AOS from 'aos';
import { UserService } from '../../../core/service/user.service';
import {RouterLink, RouterLinkActive, RouterOutlet} from "@angular/router";
import {CommonModule} from "@angular/common";

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
    RouterLinkActive
  ]
})
export class LandingPageComponent {


}
