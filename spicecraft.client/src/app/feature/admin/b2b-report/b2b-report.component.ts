import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-b2b-report',
  standalone: true,
    imports: [
        TitleComponent
    ],
  templateUrl: './b2b-report.component.html',
  styleUrl: './b2b-report.component.css'
})
export class B2bReportComponent {

}
