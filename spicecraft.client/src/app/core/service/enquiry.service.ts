import {inject, Injectable} from "@angular/core";
import {WebAPIService} from "./webAPI.service";
import {ItemFilterModel} from "../model/item/item-filter.model";
import {Observable} from "rxjs";
import {ResultDetailModel} from "../model/result-detail.model";
import {MenuItemModel} from "../model/item/menu-item.model";
import {EnquiryModel} from "../model/enquiry/enquiry-model";
import {MessageModel} from "../model/enquiry/message-model";
import {EnquiryCreationModel} from "../model/enquiry/enquiry-creation.model";
import {EnquiryTypeModel} from "../model/enquiry/enquiry-types";

@Injectable({
  providedIn: 'root'
})
export class EnquiryService {
  private _api = inject(WebAPIService)

// Fetch enquiries for a specific user
  public getEnquiriesByUser(userId: number): Observable<ResultDetailModel<EnquiryModel[]>> {
    return this._api.get<ResultDetailModel<EnquiryModel[]>>(`/enquiry/user/${userId}`);
  }

  // Fetch enquiries for internal users
  public getEnquiriesForInternalUser(userId: number): Observable<ResultDetailModel<EnquiryModel[]>> {
    return this._api.get<ResultDetailModel<EnquiryModel[]>>(`/enquiry/internal/${userId}`);
  }

  // Fetch the latest message for a specific enquiry
  public getLatestMessageByEnquiryId(enquiryId: number): Observable<ResultDetailModel<MessageModel>> {
    return this._api.get<ResultDetailModel<MessageModel>>(`/enquiry/${enquiryId}/latest-message`);
  }

  // Fetch all messages for a specific enquiry
  public getMessagesByEnquiryId(enquiryId: number): Observable<ResultDetailModel<MessageModel[]>> {
    return this._api.get<ResultDetailModel<MessageModel[]>>(`/enquiry/${enquiryId}/messages`);
  }

  // Create a new enquiry along with its first message
  public createEnquiry(enquiry: EnquiryCreationModel): Observable<ResultDetailModel<number>> {
    return this._api.post<ResultDetailModel<number>>('/enquiry', enquiry);
  }

  // Create a new message for an existing enquiry
  public createMessage(message: MessageModel): Observable<ResultDetailModel<boolean>> {
    return this._api.post<ResultDetailModel<boolean>>('/enquiry/message', message);
  }

  // Fetch a specific message by its ID
  public getMessageByMessageId(messageId: number): Observable<ResultDetailModel<MessageModel>> {
    return this._api.get<ResultDetailModel<MessageModel>>(`/enquiry/message/${messageId}`);
  }

  // Fetch all enquiry types
  public getEnquiryTypes(): Observable<ResultDetailModel<EnquiryTypeModel[]>> {
    return this._api.get<ResultDetailModel<EnquiryTypeModel[]>>(`/enquiry/enquiry-types`);
  }
}
