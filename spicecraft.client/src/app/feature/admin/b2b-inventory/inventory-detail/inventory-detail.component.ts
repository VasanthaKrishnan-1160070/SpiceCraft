import { Component } from '@angular/core';
import {TitleComponent} from "../../../../shared/components/title/title.component";

@Component({
  selector: 'sc-inventory-detail',
  standalone: true,
    imports: [
        TitleComponent
    ],
  templateUrl: './inventory-detail.component.html',
  styleUrl: './inventory-detail.component.css'
})
export class InventoryDetailComponent {

}
