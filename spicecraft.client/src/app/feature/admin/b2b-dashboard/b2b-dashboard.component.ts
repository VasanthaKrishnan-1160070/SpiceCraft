import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-b2b-dashboard',
  standalone: true,
    imports: [
        TitleComponent
    ],
  templateUrl: './b2b-dashboard.component.html',
  styleUrl: './b2b-dashboard.component.css'
})
export class B2bDashboardComponent {

}
