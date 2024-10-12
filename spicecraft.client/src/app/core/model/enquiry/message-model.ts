export interface MessageModel {
  messageId: number;
  regarding: string;
  enquiryId: number;
  messageContent: string;
  senderUserId: number;
  receiverUserId?: number;  // Nullable
  messageDate?: Date;  // Nullable
  receiver: string;
  sender: string;
  enquiryTypeId: number;
  subject: string;
}
