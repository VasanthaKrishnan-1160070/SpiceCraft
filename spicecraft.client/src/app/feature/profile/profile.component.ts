import { Component } from '@angular/core';
import {TitleComponent} from "../../shared/components/title/title.component";

@Component({
  selector: 'sc-profile',
  standalone: true,
  imports: [
    TitleComponent
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {

}
