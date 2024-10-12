import {Component, Input} from '@angular/core';
import {DxChartModule} from "devextreme-angular";
import {
    DxiSeriesModule,
    DxoCommonSeriesSettingsModule,
    DxoExportModule,
    DxoLegendModule, DxoTitleModule, DxoTooltipModule, DxoValueAxisModule
} from "devextreme-angular/ui/nested";
import {NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'sc-bar-chart',
  standalone: true,
    imports: [
        DxChartModule,
        DxiSeriesModule,
        DxoCommonSeriesSettingsModule,
        DxoExportModule,
        DxoLegendModule,
        DxoTitleModule,
        DxoTooltipModule,
        DxoValueAxisModule,
        NgForOf,
        NgIf
    ],
  templateUrl: './bar-chart.component.html',
  styleUrl: './bar-chart.component.css'
})
export class BarChartComponent {
  @Input() title!: string;
  @Input() reportData!: any[];
  @Input() seriesFields!: any[];
}
