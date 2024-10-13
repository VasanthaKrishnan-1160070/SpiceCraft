import {Component, Input, ViewChild} from '@angular/core';
import {DxDataGridComponent, DxDataGridModule} from "devextreme-angular";
// import {DxDataGridTypes} from "devextreme-angular/ui/data-grid";
// import { Workbook } from 'exceljs';
// import { saveAs } from 'file-saver-es';
// import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import * as FileSaver from 'file-saver';
import { exportDataGrid } from 'devextreme/excel_exporter'; // Import the exportDataGrid function

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
  @ViewChild('dataGrid', { static: false }) dataGrid!: DxDataGridComponent;

  @Input() title!: string;
  @Input() reportData!: any[];

  exportGrid() {
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet('Main sheet');

    exportDataGrid({
      component: this.dataGrid.instance,
      worksheet: worksheet,
      autoFilterEnabled: true,
    })
      .then(() => {
        // Customize workbook if needed

        workbook.xlsx.writeBuffer().then((buffer: BlobPart) => {
          FileSaver.saveAs(
            new Blob([buffer], { type: 'application/octet-stream' }),
            'DataGridExport.xlsx'
          );
        });
      })
      .catch((error) => {
        console.error('Error exporting grid:', error);
      });
  }

  // onExporting(e: DxDataGridTypes.ExportingEvent) {
  //   const workbook = new Workbook();
  //   const worksheet = workbook.addWorksheet('Employees');
  //
  //   exportDataGrid({
  //     component: e.component,
  //     worksheet,
  //     autoFilterEnabled: true,
  //   }).then(() => {
  //     workbook.xlsx.writeBuffer().then((buffer: any) => {
  //       saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
  //     });
  //   });
  // }
}
