import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { TitleComponent } from "../../../shared/components/title/title.component";
import {DxDataGridModule, DxFormModule, DxFormComponent, DxButtonModule, DxValidatorModule} from "devextreme-angular";
import { ActivatedRoute } from "@angular/router";
import { OrderService } from "../../../core/service/order.service";
import { UserOrderDetailModel } from "../../../core/model/order/user-order-detail.model";
import { OrderDetailModel } from "../../../core/model/order/order-detail-model";
import { ContactInfoModel } from "../../../core/model/user/contact-info.model";
import { UserAddressModel } from "../../../core/model/user/user-address.model";
import { AuthService } from "../../../core/service/auth.service";
import {take} from "rxjs/operators";
import {ResultDetailModel} from "../../../core/model/result-detail.model";
import {CommonModule, Location} from "@angular/common";
import {NotifyService} from "../../../core/service/notify.service";

@Component({
  selector: 'sc-order-detail',
  standalone: true,
  imports: [
    TitleComponent,
    DxFormModule,
    DxDataGridModule,
    DxButtonModule,
    DxValidatorModule,
    CommonModule
  ],
  templateUrl: './order-detail.component.html',
  styleUrl: './order-detail.component.css'
})
export class OrderDetailComponent implements OnInit {

  @ViewChild(DxFormComponent, { static: false }) formInstance!: DxFormComponent;

  private _orderService: OrderService = inject(OrderService);
  private _authService: AuthService = inject(AuthService);
  private _notifyService = inject(NotifyService);
  private _locationService = inject(Location);

  orderId!: number;
  order: any = {};  // Order information
  orderDetails!: UserOrderDetailModel;  // Ordered items
  orderItems!: OrderDetailModel[];
  contactInfo!: ContactInfoModel;  // Contact information
  userAddress!: UserAddressModel;  // User address
  isInternalUser = false;
  orderStatusOptions: any[] = [];  // Holds the order status options for the select box

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.orderId = +(params.get('orderId') || 0);
      this.getOrderDetails(this.orderId);
    });
    this.isInternalUser = this._authService.isInternalUser();

    // Fetch order statuses
    this.orderStatusOptions = this._orderService.getAllOrderStatus();
  }

  // Fetch order details from the API
  getOrderDetails(orderId: number) {
    this._orderService
        .getOrderDetails(orderId)
      .pipe(
        take(1),
      )
      .subscribe((response: ResultDetailModel<UserOrderDetailModel>) => {
      this.orderDetails = response.data as UserOrderDetailModel;
      this.contactInfo = this.orderDetails?.contactInfo as ContactInfoModel;
      this.userAddress = this.orderDetails?.shippingAddress as UserAddressModel;
      this.orderItems = this.orderDetails?.orderItems as OrderDetailModel[];
    }, error => {
      console.error('Error fetching order details', error);
    });
  }

  goBack() {
    this._locationService.back();
  }

  // Add a new item to the order
  addItem() {
    const newItem: OrderDetailModel = {
      itemName: 'New Item',
      quantity: 1,
      price: 0,
      itemId: 0,
      orderId: this.orderId,
      purchasePrice: 0
    };

    this.orderDetails.orderItems.push(newItem);
  }

  // Save or update order details including the order status, contact info, address, and order items
  saveOrder() {
    const validationResult = this.formInstance.instance.validate();
    if (!validationResult.isValid) {
      console.error('Validation failed');
      return;
    }

    const updatedOrder: UserOrderDetailModel = {
      ...this.orderDetails,
      orderStatus: this.orderDetails.orderStatus,  // Include the updated status
      contactInfo: this.orderDetails.contactInfo,  // Include updated contact info
      shippingAddress: this.orderDetails.shippingAddress,  // Include updated address
      orderItems: this.orderDetails.orderItems  // Include updated order items
    };

    this._orderService.updateOrder(this.orderId, updatedOrder).subscribe(response => {
      this.getOrderDetails(this.orderId);
      this._notifyService.showSuccess("Order updated successfully.");
      console.log('Order updated successfully');
    }, error => {
      console.error('Error updating order', error);
    });
  }
}
