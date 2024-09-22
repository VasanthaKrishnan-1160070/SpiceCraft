export interface PaymentModel {
  transactionId: number;
  paymentAmount: string;  // Payment amount as a string formatted with '$'
  orderId: number;
  paymentStatus: string;
  paymentDate: string;  // Date formatted as 'dd/MM/yyyy'
}
