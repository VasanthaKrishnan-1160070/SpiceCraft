import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-b2b-settings',
  standalone: true,
    imports: [
        TitleComponent
    ],
  templateUrl: './b2b-settings.component.html',
  styleUrl: './b2b-settings.component.css'
})
export class B2bSettingsComponent {

}
