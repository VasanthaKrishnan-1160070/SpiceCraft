import {Component, Input} from '@angular/core';
import {DxPieChartModule} from "devextreme-angular";
import {NgForOf} from "@angular/common";

@Component({
  selector: 'sc-doughnut-chart',
  standalone: true,
  imports: [
    DxPieChartModule,
    NgForOf
  ],
  templateUrl: './doughnut-chart.component.html',
  styleUrl: './doughnut-chart.component.css'
})
export class DoughnutChartComponent {
  @Input() title!: string;
  @Input() reportData!: any[];
  @Input() seriesFields!: any[];

  public pointClickHandler(arg: any) {
    arg.target.select();
  }
}
