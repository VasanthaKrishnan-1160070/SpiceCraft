import { Component } from '@angular/core';
import {PaymentDetailComponent} from "../payment-detail/payment-detail.component";

@Component({
  selector: 'sc-payment-list',
  standalone: true,
  imports: [
    PaymentDetailComponent
  ],
  templateUrl: './payment-list.component.html',
  styleUrl: './payment-list.component.css'
})
export class PaymentListComponent {

}
