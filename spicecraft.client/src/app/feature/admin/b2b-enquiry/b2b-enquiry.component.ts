import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {EnquiryListComponent} from "../../enquiry/enquiry-list/enquiry-list.component";

@Component({
  selector: 'sc-b2b-enquiry',
  standalone: true,
  imports: [
    TitleComponent,
    EnquiryListComponent
  ],
  templateUrl: './b2b-enquiry.component.html',
  styleUrl: './b2b-enquiry.component.css'
})
export class B2bEnquiryComponent {

}
