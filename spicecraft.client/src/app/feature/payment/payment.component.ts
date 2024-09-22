import { Component } from '@angular/core';
import {TitleComponent} from "../../shared/components/title/title.component";
import {PaymentListComponent} from "./payment-list/payment-list.component";

@Component({
  selector: 'sc-payment',
  standalone: true,
  imports: [
    TitleComponent,
    PaymentListComponent
  ],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent {

}
