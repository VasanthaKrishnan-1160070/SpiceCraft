import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-b2b-customer',
  standalone: true,
    imports: [
        TitleComponent
    ],
  templateUrl: './b2b-customer.component.html',
  styleUrl: './b2b-customer.component.css'
})
export class B2bCustomerComponent {

}
