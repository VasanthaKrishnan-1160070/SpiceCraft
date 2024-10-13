import {Component, OnInit} from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {RouterLink} from "@angular/router";
import {CurrencyPipe, NgIf} from "@angular/common";
import {DxDataGridModule} from "devextreme-angular";
import {InventoryService} from "../../../core/service/inventory.service";
import {OrderService} from "../../../core/service/order.service";
import {ResultDetailModel} from "../../../core/model/result-detail.model";
import {take} from "rxjs/operators";

@Component({
  selector: 'sc-b2b-dashboard',
  standalone: true,
  imports: [
    TitleComponent,
    RouterLink,
    NgIf,
    DxDataGridModule,
    CurrencyPipe
  ],
  templateUrl: './b2b-dashboard.component.html',
  styleUrl: './b2b-dashboard.component.css'
})
export class B2bDashboardComponent implements OnInit {

  public profileImage: string = '';
  public currentUserName!: string;
  public newMessagesCount: number = 0;
  public last7dayOrdersCount: number = 0;
  public unshippedOrdersCount: number = 0;
  public lowStockProducts: number = 0;

  public todaysOrders: number = 0;
  public ordersToShip: number = 0;
  public totalSalesToday: number = 0;
  public totalSalesMonth: number = 0;
  public lowStockIngredients!: any;

  constructor(private orderService: OrderService, private inventoryService: InventoryService) {}

  ngOnInit(): void {
    this.loadTodaysOrdersCount();
    this.loadOrdersToShipCount();
    this.loadTotalSalesToday();
    this.loadTotalSalesMonth();
    this.loadLowStockIngredients();
  }

  loadTodaysOrdersCount() {
    this.orderService.getTodaysOrdersCount()
      .pipe(take(1))
      .subscribe((result: ResultDetailModel<number>) => {
      if (result.isSuccess) {
        this.todaysOrders = result.data as number;
      } else {
        console.error(result.message);
      }
    });
  }

  loadOrdersToShipCount() {
    this.orderService.getOrdersToShipCount()
      .pipe(take(1))
      .subscribe((result: ResultDetailModel<number>) => {
      if (result.isSuccess) {
        this.ordersToShip = result.data as number;
      } else {
        console.error(result.message);
      }
    });
  }

  loadTotalSalesToday() {
    this.orderService.getTotalSalesToday()
      .pipe(take(1))
      .subscribe((result: ResultDetailModel<number>) => {
      if (result.isSuccess) {
        this.totalSalesToday = result.data as number;
      } else {
        console.error(result.message);
      }
    });
  }

  loadTotalSalesMonth() {
    this.orderService.getTotalSalesMonth()
      .pipe(take(1))
      .subscribe((result: ResultDetailModel<number>) => {
      if (result.isSuccess) {
        this.totalSalesMonth = result.data as number;
      } else {
        console.error(result.message);
      }
    });
  }

  loadLowStockIngredients() {
    this.inventoryService.getLowStockIngredients()
      .pipe(take(1))
      .subscribe(s => {
        this.lowStockIngredients = s.data;
      })
  }

}
