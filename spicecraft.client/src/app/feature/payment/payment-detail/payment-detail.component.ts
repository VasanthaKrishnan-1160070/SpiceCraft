import {Component, inject, OnInit} from '@angular/core';
import {DxButtonModule, DxDataGridModule, DxFormModule} from "devextreme-angular";
import {TitleComponent} from "../../../shared/components/title/title.component";
import {PaymentInvoiceModel} from "../../../core/model/payment/payment-invoice.model";
import {PaymentService} from "../../../core/service/payment.service";
import {Subject} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import {ContactInfoModel} from "../../../core/model/user/contact-info.model";
import {UserAddressModel} from "../../../core/model/user/user-address.model";
import {OrderDetailModel} from "../../../core/model/order/order-detail-model";
import {CommonModule, CurrencyPipe} from "@angular/common";
import {AuthService} from "../../../core/service/auth.service";
import jsPDF from "jspdf";
import html2canvas from "html2canvas";

@Component({
  selector: 'sc-payment-detail',
  standalone: true,
  imports: [
    DxDataGridModule,
    DxFormModule,
    TitleComponent,
    CurrencyPipe,
    CommonModule,
    DxButtonModule
  ],
  templateUrl: './payment-detail.component.html',
  styleUrl: './payment-detail.component.css'
})
export class PaymentDetailComponent implements OnInit {
  paymentInfo?: PaymentInvoiceModel | undefined | null;
  orderPayments: any = {};  // Order payment details
  contactInfo!: ContactInfoModel | null | undefined;    // Contact information
  userAddress!: UserAddressModel | null | undefined;    // Shipping address
  orderDetails: OrderDetailModel[] = []; // List of order items
  isInternalUser = false;

  private _paymentService: PaymentService = inject(PaymentService);
  private _destroy$: Subject<void> = new Subject<void>();
  private _router: ActivatedRoute = inject(ActivatedRoute);
  private _authService = inject(AuthService);

  ngOnInit(): void {
    this.isInternalUser = this._authService.isInternalUser();
    this._router.paramMap.subscribe(params => {
      const transactionId = +(params.get('transactionId') || 0);
      this.loadPaymentDetails(transactionId);
    });
  }

  loadPaymentDetails(transactionId: number): void {
    //  Load data for orderPayments, contactInfo, userAddress, orderDetails
    this._paymentService.getPaymentInvoiceDetails(transactionId)
      .subscribe(response => {
        this.paymentInfo = response.data;
        this.contactInfo = response.data?.contactInfo;
        this.userAddress = response.data?.userAddress;
        this.orderDetails = response.data?.orderDetails as OrderDetailModel[];
      });
  }

  downloadInvoice() {
    const doc = new jsPDF();

    // Add the styled "SpiceCraft" logo-like text at the top-left corner in italics
    doc.setFont('courier', 'italic');  // Set font style to Italic
    doc.setFontSize(30);  // Large font size for SpiceCraft
    doc.setTextColor('#D2691E');  // SpiceCraft color (brownish-orange)

    // Add "SpiceCraft" text at the top-left
    doc.text('SpiceCraft', 10, 20);  // X: 10, Y: 20 for top-left positioning

    // Reset font style to normal for the rest of the document
    doc.setFont('courier', 'normal');  // Switch back to normal font
    doc.setFontSize(18);  // Set font size for headings

    // Set Steel Blue color for headings
    doc.setTextColor('#4682B4');  // Steel Blue color for Invoice

    // Calculate the text width and center the "Invoice" text
    const pageWidth = doc.internal.pageSize.getWidth();  // Get the page width
    const invoiceText = 'Invoice';
    const textWidth = doc.getTextWidth(invoiceText);
    const textX = (pageWidth - textWidth) / 2;  // Center the text

    // Add Invoice text in the center
    doc.text(invoiceText, textX, 40);  // Adjusted Y-coordinate to give space for the logo

    // Reset to black for body text
    doc.setTextColor(0, 0, 0);
    doc.setFontSize(12);

    // Add some details about the payment
    doc.text(`Transaction ID: ${this.paymentInfo?.transactionId}`, 10, 60);  // Add space before each section
    doc.text(`Name: ${this.contactInfo?.name}`, 10, 70);
    doc.text(`Email: ${this.contactInfo?.email}`, 10, 80);
    doc.text(`Phone: ${this.contactInfo?.phone}`, 10, 90);

    // Add space between sections
    let yOffset = 110;

    // Add User Address (if available)
    if (this.userAddress) {
      doc.setTextColor('#4682B4');  // Steel Blue color for address heading
      doc.text('Shipping Address:', 10, yOffset);

      // Reset to black for address text
      doc.setTextColor(0, 0, 0);
      doc.text(`${this.userAddress.streetAddress1}`, 10, yOffset + 10);
      if (this.userAddress.streetAddress2) {
        doc.text(`${this.userAddress.streetAddress2}`, 10, yOffset + 20);
      }
      doc.text(`${this.userAddress.city}, ${this.userAddress.stateOrProvince}, ${this.userAddress.postalCode}`, 10, yOffset + 30);

      yOffset += 50;  // Increase space after address
    }

    // Add table header for order items
    doc.setTextColor('#4682B4');  // Steel Blue color for items heading
    doc.setFontSize(14);
    doc.text('Order Details', 10, yOffset);

    yOffset += 10;

    // Draw table headers
    doc.setFontSize(12);
    doc.setTextColor(0, 0, 0);
    doc.text('Item Name', 10, yOffset);
    doc.text('Qty', 80, yOffset);
    doc.text('Price', 120, yOffset);
    doc.text('Total', 160, yOffset);

    // Draw a line under the table headers
    doc.line(10, yOffset + 2, 200, yOffset + 2);

    yOffset += 10;  // Move to the next line

    // Add order items in table format
    this.orderDetails.forEach(item => {
      doc.text(`${item.itemName}`, 10, yOffset);
      doc.text(`${item.quantity}`, 80, yOffset);
      doc.text(`$${item.price}`, 120, yOffset);
      doc.text(`$${(item.price * item.quantity).toFixed(2)}`, 160, yOffset);
      yOffset += 10;
    });

    // Add space before totals
    yOffset += 10;

    // Add Subtotal, Shipping Cost, and Total
    doc.setFontSize(12);
    doc.text(`Subtotal: $${this.paymentInfo?.subTotal?.toFixed(2)}`, 10, yOffset);
    yOffset += 10;
    doc.text(`Shipping Cost: $${this.paymentInfo?.shippingCost?.toFixed(2)}`, 10, yOffset);
    yOffset += 10;

    // Add Total Amount
    doc.setTextColor('#4682B4');  // Steel Blue color for total heading
    doc.text(`Total Amount: $${this.paymentInfo?.amount?.toFixed(2)}`, 10, yOffset + 10);

    // Download the generated PDF
    doc.save(`invoice_${this.paymentInfo?.transactionId}.pdf`);
  }










  downloadInvoice2() {
    const element = document.getElementById('payment-details');  // Capture the HTML section by its ID

    html2canvas(element!).then(canvas => {
      const imgData = canvas.toDataURL('image/png');
      const doc = new jsPDF();

      // Add the captured image to the PDF
      doc.addImage(imgData, 'PNG', 10, 10, 100, 100);

      // Download the generated PDF
      doc.save(`invoice_${this.paymentInfo?.userId}.pdf`);
    });
  }

}
