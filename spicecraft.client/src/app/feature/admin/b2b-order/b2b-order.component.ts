import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-b2b-order',
  standalone: true,
    imports: [
        TitleComponent
    ],
  templateUrl: './b2b-order.component.html',
  styleUrl: './b2b-order.component.css'
})
export class B2bOrderComponent {

}
