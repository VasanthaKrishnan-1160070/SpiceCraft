import {Component, Input} from '@angular/core';
import {DxDataGridModule} from "devextreme-angular";

@Component({
  selector: 'sc-report-grid',
  standalone: true,
  imports: [
    DxDataGridModule
  ],
  templateUrl: './report-grid.component.html',
  styleUrl: './report-grid.component.css'
})
export class ReportGridComponent {
  @Input() title!: string;
  @Input() reportData!: any[];
}
