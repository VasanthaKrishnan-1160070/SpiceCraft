import { Component } from '@angular/core';
import {TitleComponent} from "../../shared/components/title/title.component";

@Component({
  selector: 'sc-gift-card',
  standalone: true,
  imports: [
    TitleComponent
  ],
  templateUrl: './gift-card.component.html',
  styleUrl: './gift-card.component.css'
})
export class GiftCardComponent {

}
