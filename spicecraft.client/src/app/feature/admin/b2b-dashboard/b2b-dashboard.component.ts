import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {RouterLink} from "@angular/router";
import {NgIf} from "@angular/common";

@Component({
  selector: 'sc-b2b-dashboard',
  standalone: true,
  imports: [
    TitleComponent,
    RouterLink,
    NgIf
  ],
  templateUrl: './b2b-dashboard.component.html',
  styleUrl: './b2b-dashboard.component.css'
})
export class B2bDashboardComponent {

  public profileImage: string = '';
  public currentUserName!: string;
  public newMessagesCount: number = 0;
  public last7dayOrdersCount: number = 0;
  public unshippedOrdersCount: number = 0;
  public lowStockProducts: number = 0;

}
