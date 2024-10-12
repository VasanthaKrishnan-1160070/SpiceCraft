import {Component, Input, OnChanges, SimpleChanges} from '@angular/core';
import {DxSparklineModule} from "devextreme-angular";
import {DxiSeriesModule, DxoConnectorModule, DxoLabelModule} from "devextreme-angular/ui/nested";
import {NgForOf} from "@angular/common";

@Component({
  selector: 'sc-sparkline-chart',
  standalone: true,
  imports: [
    DxSparklineModule,
    DxiSeriesModule,
    DxoConnectorModule,
    DxoLabelModule,
    NgForOf
  ],
  templateUrl: './sparkline-chart.component.html',
  styleUrl: './sparkline-chart.component.css'
})
export class SparklineChartComponent implements OnChanges {

  @Input() title!: string;
  @Input() reportData!: any[];
  @Input() seriesFields!: any[];

  argumentField = '';
  valueField = '';

  ngOnChanges(changes: SimpleChanges): void {
    const seriesFields = changes['seriesFields'].currentValue;
    if (seriesFields.length > 0) {
      this.argumentField = seriesFields[0].argumentField;
      this.valueField = seriesFields[0].valueField;
    }
  }

}
