import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-b2b-inventory',
  standalone: true,
    imports: [
        TitleComponent
    ],
  templateUrl: './b2b-inventory.component.html',
  styleUrl: './b2b-inventory.component.css'
})
export class B2bInventoryComponent {

}
