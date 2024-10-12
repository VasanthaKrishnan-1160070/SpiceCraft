import {Component, OnInit} from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {DxButtonModule, DxChartModule, DxDataGridModule, DxFormModule, DxSelectBoxModule} from "devextreme-angular";
import {ReportService} from "../../../core/service/report.service";
import {DxiValueAxisModule, DxoValueAxisModule} from "devextreme-angular/ui/nested";
import {CommonModule, NgForOf} from "@angular/common";
import {BarChartComponent} from "./bar-chart/bar-chart.component";
import {PieChartComponent} from "./pie-chart/pie-chart.component";
import {FunnelPyramidChartComponent} from "./funnel-pyramid-chart/funnel-pyramid-chart.component";
import {SparklineChartComponent} from "./sparkline-chart/sparkline-chart.component";
import {DoughnutChartComponent} from "./doughnut-chart/doughnut-chart.component";
import {ReportGridComponent} from "./report-grid/report-grid.component";

@Component({
  selector: 'sc-b2b-report',
  standalone: true,
  imports: [
    TitleComponent,
    DxChartModule,
    DxiValueAxisModule,
    DxoValueAxisModule,
    DxDataGridModule,
    DxSelectBoxModule,
    NgForOf,
    CommonModule,
    DxButtonModule,
    BarChartComponent,
    PieChartComponent,
    FunnelPyramidChartComponent,
    SparklineChartComponent,
    DoughnutChartComponent,
    ReportGridComponent,
    DxFormModule
  ],
  templateUrl: './b2b-report.component.html',
  styleUrl: './b2b-report.component.css'
})
export class B2bReportComponent implements OnInit {
  reportOptions = [
    { name: 'Monthly Order Summary', value: 'monthly-order-summary' },
    { name: 'Monthly Sales Summary', value: 'monthly-sales-summary' },
    { name: 'Category Sales Summary', value: 'category-sales-summary' },
    { name: 'Payment Method Summary', value: 'payment-method-summary' },
    { name: 'Product Sales by Month', value: 'product-sales-by-month' },
    { name: 'Most Sold Products', value: 'most-sold-products' }
  ];
  chartTypes = ['bar', 'pie', 'funnel', 'sparkline','doughnut'];

  selectedReport!: string;
  selectedChartType: string = 'bar';
  reportData!: any[];
  chartData!: [];
  gridColumns: string[] = [];
  seriesFields: any[] = [];
  formData: any = {};
  runButtonOptions: any;

  constructor(private reportService: ReportService) {
    this.runButtonOptions = {
      text: 'Run Report',
      type: 'success',
      class: 'w-25',
      onClick: this.runReport.bind(this),
    };
  }

  ngOnInit(): void {}

  runReport(): void {
    this.selectedReport = this.formData.selectedReport;
    this.selectedChartType = this.formData.selectedChartType;

    if (this.selectedReport) {
      this.reportService.getReportByName(this.selectedReport).subscribe((result: any) => {
        this.reportData = result.data; // Assuming the data is in the `result` field
        this.setupSeriesFields();
      });
    }
  }

  setupSeriesFields() {
    if (this.reportData && this.reportData.length > 0) {

     switch (this.selectedReport) {
       case 'monthly-order-summary':
         this.seriesFields = [{ argumentField: 'monthName', valueField: 'orderCount', name: 'Orders by Month'}];
         break;
       case 'monthly-sales-summary':
         this.seriesFields = [{ argumentField: 'monthName', valueField: 'totalSales', name: 'Sales by Month'}];
         break;
       case 'category-sales-summary':
         this.seriesFields = [{ argumentField: 'categoryName', valueField: 'totalSales', name: 'Sales by Month'}];
         break;
       case 'payment-method-summary':
         this.seriesFields = [{ argumentField: 'paymentMethod', valueField: 'totalAmount', name: 'Payment Method Summary'}];
         break;
       case 'product-sales-by-mont':
         this.seriesFields = [{ argumentField: 'monthName', valueField: 'totalSales', name: 'Product Sales by Month'}];
         break;
       case 'most-sold-products':
         this.seriesFields = [{ argumentField: 'itemName', valueField: 'totalSales', name: 'Top 10 Product Sales'}];
         break;
     }
    }
  }
}
