import {Component, input, Input} from '@angular/core';

@Component({
  selector: 'sc-title',
  standalone: true,
  imports: [],
  templateUrl: './title.component.html',
  styleUrl: './title.component.css'
})
export class TitleComponent {
 title = input<string>('');
}
