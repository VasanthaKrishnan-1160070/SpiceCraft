import { Component } from '@angular/core';
import {CommonModule} from "@angular/common";
import {DxButtonModule} from "devextreme-angular";
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    DxButtonModule,
    TitleComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

}
