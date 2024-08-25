import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-b2b-payment',
  standalone: true,
    imports: [
        TitleComponent
    ],
  templateUrl: './b2b-payment.component.html',
  styleUrl: './b2b-payment.component.css'
})
export class B2bPaymentComponent {

}
