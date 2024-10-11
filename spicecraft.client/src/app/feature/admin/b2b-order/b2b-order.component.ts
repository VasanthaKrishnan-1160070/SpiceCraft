import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {OrderListComponent} from "../../order/order-list/order-list.component";

@Component({
  selector: 'sc-b2b-order',
  standalone: true,
  imports: [
    TitleComponent,
    OrderListComponent
  ],
  templateUrl: './b2b-order.component.html',
  styleUrl: './b2b-order.component.css'
})
export class B2bOrderComponent {

}
