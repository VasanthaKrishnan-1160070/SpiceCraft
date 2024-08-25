import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-b2b-profile',
  templateUrl: './b2b-profile.component.html',
  styleUrl: './b2b-profile.component.css',
  standalone: true,
  imports:[
    TitleComponent
  ]
})
export class B2bProfileComponent {

}
