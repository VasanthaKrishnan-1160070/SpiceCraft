import {Component, Input} from '@angular/core';
import {DxPieChartModule} from "devextreme-angular";
import {NgForOf} from "@angular/common";

@Component({
  selector: 'sc-pie-chart',
  standalone: true,
  imports: [
    DxPieChartModule,
    NgForOf
  ],
  templateUrl: './pie-chart.component.html',
  styleUrl: './pie-chart.component.css'
})
export class PieChartComponent {
  @Input() title!: string;
  @Input() reportData!: any[];
  @Input() seriesFields!: any[];

  pointClickHandler(e: any) {
    this.toggleVisibility(e.target);
  }

  legendClickHandler(e: any) {
    const arg = e.target;
    const item = e.component.getAllSeries()[0].getPointsByArg(arg)[0];

    this.toggleVisibility(item);
  }

  toggleVisibility(item: any) {
    if (item.isVisible()) {
      item.hide();
    } else {
      item.show();
    }
  }
}
