import { Component } from '@angular/core';
import {TitleComponent} from "../../shared/components/title/title.component";

@Component({
  selector: 'sc-payment',
  standalone: true,
  imports: [
    TitleComponent
  ],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent {

}
