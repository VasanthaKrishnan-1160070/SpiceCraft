import { Component } from '@angular/core';
import {TitleComponent} from "../../shared/components/title/title.component";

@Component({
  selector: 'sc-enquiry',
  standalone: true,
  imports: [
    TitleComponent
  ],
  templateUrl: './enquiry.component.html',
  styleUrl: './enquiry.component.css'
})
export class EnquiryComponent {

}
