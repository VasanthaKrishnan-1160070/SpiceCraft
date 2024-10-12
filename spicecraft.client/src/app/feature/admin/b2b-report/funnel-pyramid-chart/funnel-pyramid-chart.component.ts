import {Component, Input, OnChanges, SimpleChanges} from '@angular/core';
import {DxFunnelModule} from "devextreme-angular";
import {DxiSeriesModule} from "devextreme-angular/ui/nested";
import {NgForOf} from "@angular/common";

@Component({
  selector: 'sc-funnel-pyramid-chart',
  standalone: true,
  imports: [
    DxFunnelModule,
    DxiSeriesModule,
    NgForOf
  ],
  templateUrl: './funnel-pyramid-chart.component.html',
  styleUrl: './funnel-pyramid-chart.component.css'
})
export class FunnelPyramidChartComponent implements OnChanges{
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

//   customizeText ({
//                      percentText,
//                      item: {argument}
//                    }) {
//
//   return `<span style='font-size: 28px'>${percentText?.toString()}</span><br/>${argument}`;
//
// }
}

