import { Component } from '@angular/core';
import {CommonModule} from "@angular/common";
import {DxButtonModule} from "devextreme-angular";

@Component({
  selector: 'sc-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    DxButtonModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

}
