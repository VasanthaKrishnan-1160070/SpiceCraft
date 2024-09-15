export interface EnquiryCreationModel {
  enquiryTypeId: number; // Enquiry type
  initialMessage: string; // Initial message content
  senderUserId: number; // ID of the sender
  receiverUserId?: number; // Optional receiver ID (nullable in C#)
}
