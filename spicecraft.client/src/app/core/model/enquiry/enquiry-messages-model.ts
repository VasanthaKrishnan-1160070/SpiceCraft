import {MessageModel} from "./message-model";

export interface EnquiryMessagesModel {
  enquiryMessages: MessageModel[];
  latestMessage: MessageModel;
}
