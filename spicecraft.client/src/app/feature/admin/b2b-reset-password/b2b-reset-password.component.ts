import { Component } from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";

@Component({
  selector: 'sc-b2b-reset-password',
  standalone: true,
    imports: [
        TitleComponent
    ],
  templateUrl: './b2b-reset-password.component.html',
  styleUrl: './b2b-reset-password.component.css'
})
export class B2bResetPasswordComponent {

}
