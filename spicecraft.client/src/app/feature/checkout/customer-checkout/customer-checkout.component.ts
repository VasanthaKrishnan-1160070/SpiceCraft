import {Component, OnInit} from '@angular/core';
import {Router, RouterLink} from "@angular/router";
import {DxButtonModule, DxFormModule, DxPopupModule, DxSelectBoxModule, DxTextBoxModule} from "devextreme-angular";
import {FormsModule} from "@angular/forms";
import {CheckoutService} from "../../../core/service/checkout.service";
import {StartPostPaymentModel} from "../../../core/model/checkout/start-post-payment.model";
import {UserService} from "../../../core/service/user.service";
import {ShippingOptionModel} from "../../../core/model/shipping/shipping-option-model";
import {CommonModule} from "@angular/common";
import notify from "devextreme/ui/notify";
import {NotifyService} from "../../../core/service/notify.service";

@Component({
  selector: 'sc-customer-checkout',
  templateUrl: './customer-checkout.component.html',
  styleUrl: './customer-checkout.component.css',
  imports: [
    RouterLink,
    DxButtonModule,
    DxPopupModule,
    DxFormModule,
    DxTextBoxModule,
    FormsModule,
    DxSelectBoxModule,
    CommonModule
  ],
  standalone: true
})
export class CustomerCheckoutComponent implements OnInit {
  userAddress: any; // Replace with your user address model
  subTotal: number = 0;
  shippingCost: number = 0;
  total: number = 0;
  totalDue: number = 0;
  gstAmount: number = 0;
  giftCardBalance: number = 0;
  giftCardCode: string = '';
  selectedShipping: number = 0;
  isEligibleForFreeShipping: boolean = false;
  amountNeededToReachFreeShipping: number = 0;
  shippingOptions: ShippingOptionModel[] = []; // Replace with your shipping option model
  countryName: string = '';
  errorMessage: string = '';
  isCreditCardPopupVisible: boolean = false;

  creditCardForm: any = {
    amountToPay: '',
    cardHolderName: '',
    cardNumber: '',
    expiryDate: '',
    cvv: ''
  };

  constructor(
    private checkoutService: CheckoutService,
    private userService: UserService,
    private notifyService: NotifyService,
    private router: Router
    ) {}

  ngOnInit(): void {
    this.loadCheckoutDetails();
  }

  private get userId()  {
    return this.userService.getCurrentUserId();
  }

  loadCheckoutDetails() {
    this.checkoutService.getCheckoutInfo(this.userId).subscribe(details => {
      this.userAddress = details.data?.userAddress;
      this.subTotal = details.data?.subTotal ?? 0;
      this.shippingOptions = details.data?.shippingOptions || [];
      this.selectedShipping = details.data?.shippingOptions[0]?.shippingOptionId || 0;
      this.calculateTotal();
    });
  }

  onShippingOptionChange() {
    this.calculateTotal();
  }

  calculateTotal() {
    this.shippingCost = this.getSelectedShippingCost();
    this.isEligibleForFreeShipping = this.canDoFreeShipping();
    if (this.isEligibleForFreeShipping) {
      this.shippingCost = 0.00;
    }
    this.amountNeededToReachFreeShipping = this.getPriceNeededToReachFreeShipping();
    this.total = +(this.subTotal + this.shippingCost).toFixed(2);
    this.totalDue = +(this.total - this.giftCardBalance)?.toFixed(2);
    this.gstAmount = this.calculateGst(this.totalDue);
    this.totalDue += this.gstAmount;
    this.totalDue = +this.totalDue.toFixed(2);
    this.creditCardForm.amountToPay = '$'+this.totalDue ;
  }

  getSelectedShippingCost(): number {
    const selectedOption = this.shippingOptions.find(option => option.shippingOptionId === this.selectedShipping);
    return selectedOption ? selectedOption.cost : 0;
  }

  canDoFreeShipping(): boolean {
    const selectedOption = this.shippingOptions.find(option => option.shippingOptionId === this.selectedShipping);
    return selectedOption ? this.subTotal >= selectedOption.freeShippingThreshold : false;
  }

  getPriceNeededToReachFreeShipping(): number {
    const selectedOption = this.shippingOptions.find(option => option.shippingOptionId === this.selectedShipping);
    return selectedOption ? +Math.max(0, selectedOption.freeShippingThreshold - this.subTotal)?.toFixed(2) : 0;
  }

  calculateGst(totalAmount: number): number {
    return +(totalAmount * 0.15)?.toFixed(2); // Assuming a 15% GST rate
  }

  applyGiftCard() {
    this.checkoutService.applyGiftCard(this.giftCardCode, this.userId).subscribe(response => {
      if (response.isSuccess) {
      //  this.giftCardBalance = response.data.balance;
        this.calculateTotal();
      } else {
        this.errorMessage = response.message;
      }
    });
  }

  placeOrder(paymentMethod: string) {

    const orderDetails: StartPostPaymentModel = {
      shippingOptionId: this.selectedShipping,
      paymentMethod: paymentMethod,
      giftCardCode: this.giftCardCode,
      userId: this.userId
    };

    this.checkoutService
        .startPostPaymentProcess(orderDetails)
        .subscribe(response => {
      if (response.isSuccess) {
        // Handle success, e.g., redirect to order confirmation page
        this.isCreditCardPopupVisible = false;
        this.notifyService.showSuccess(response.message);
        this.router.navigate(['/order-list']);

      } else {
        this.errorMessage = response.message;
        this.isCreditCardPopupVisible = false;
        this.notifyService.showSuccess(response.message);
      }
    });
  }

  openCreditCardPopup() {
    this.isCreditCardPopupVisible = true;
  }

  closeCreditCardPopup() {
    this.isCreditCardPopupVisible = false;
  }
}
