import { Component } from '@angular/core';
import {TitleComponent} from "../../shared/components/title/title.component";

@Component({
  selector: 'sc-returns',
  standalone: true,
  imports: [
    TitleComponent
  ],
  templateUrl: './returns.component.html',
  styleUrl: './returns.component.css'
})
export class ReturnsComponent {

}
