import {Component, inject, OnInit} from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {DxDataGridModule, DxFormModule} from "devextreme-angular";
import {ActivatedRoute} from "@angular/router";
import {OrderService} from "../../../core/service/order.service";
import {UserOrderDetailModel} from "../../../core/model/order/user-order-detail.model";
import {OrderDetailModel} from "../../../core/model/order/order-detail-model";
import {ContactInfoModel} from "../../../core/model/user/contact-info.model";
import {UserAddressModel} from "../../../core/model/user/user-address.model";

@Component({
  selector: 'sc-order-detail',
  standalone: true,
  imports: [
    TitleComponent,
    DxFormModule,
    DxDataGridModule
  ],
  templateUrl: './order-detail.component.html',
  styleUrl: './order-detail.component.css'
})
export class OrderDetailComponent implements OnInit {

  private _orderService: OrderService = inject(OrderService);

  orderId!: number;
  order: any = {};  // Order information
  orderDetails!: UserOrderDetailModel;  // Ordered items
  orderItems!: OrderDetailModel[];
  contactInfo!: ContactInfoModel;  // Contact information
  userAddress!: UserAddressModel;  // User address

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.orderId = +(params.get('orderId') || 0);
      this.getOrderDetails(this.orderId);
    });
  }

  // Fetch order details from the API
  getOrderDetails(orderId: number) {
    this._orderService.getOrderDetails(orderId).subscribe((response: any) => {
      const orderInfo = response.data;
      this.orderDetails = orderInfo;
      this.orderItems = orderInfo.orderDetails;
      this.contactInfo = orderInfo.contactInfo;
      this.userAddress = orderInfo.shippingAddress;
    }, error => {
      console.error('Error fetching order details', error);
    });
  }
}
