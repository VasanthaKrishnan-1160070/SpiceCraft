import { Component, OnInit } from '@angular/core';
import {AsyncPipe, DatePipe, NgForOf, NgIf} from "@angular/common";
import { FormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { EnquiryService } from "../../../core/service/enquiry.service";
import { ResultDetailModel } from "../../../core/model/result-detail.model";
import {MessageModel} from "../../../core/model/enquiry/message-model";
import {EnquiryModel} from "../../../core/model/enquiry/enquiry-model";
import {EnquiryTypeModel} from "../../../core/model/enquiry/enquiry-types";
import {mergeMap, Observable} from "rxjs";
import {map} from "rxjs/operators";

@Component({
  selector: 'sc-enquiry-detail',
  standalone: true,
  imports: [
    NgForOf,
    FormsModule,
    NgIf,
    AsyncPipe,
    DatePipe
  ],
  templateUrl: './enquiry-detail.component.html',
  styleUrls: ['./enquiry-detail.component.css']
})
export class EnquiryDetailComponent implements OnInit {
  messages: MessageModel[] = [];  // This will store the enquiry messages
  enquiry: EnquiryModel[] = [];  // This will store enquiries for the user
  replyInfo: MessageModel | null | undefined = null;  // This will store the details of the latest message
  currentUserId: number = 1;
  enquiryTypes$!: Observable<EnquiryTypeModel[]>;

  replyMessageContent: string = '';  // The content for the reply message
  enquiryId: number | null = null;  // Store the enquiry ID

  constructor(
    private enquiryService: EnquiryService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Get the enquiry ID from the route parameters
    this.enquiryId = Number(this.route.snapshot.paramMap.get('id'));

    // Fetch data from the service
    if (this.enquiryId) {
      this.getEnquiryTypes();
      this.getMessagesByEnquiryId(this.enquiryId);
      this.getLatestMessageByEnquiryId(this.enquiryId);
    }
  }

  public getEnquiryTypes(){
    this.enquiryTypes$ = this.enquiryService.getEnquiryTypes().pipe(
      map(result =>  result.data as EnquiryTypeModel[] )
    );
  }

  // Fetch all messages for the enquiry
  getMessagesByEnquiryId(enquiryId: number) {
    this.enquiryService.getMessagesByEnquiryId(enquiryId).subscribe(
      (response: ResultDetailModel<MessageModel[]>) => {
        if (response.isSuccess) {
          this.messages = response.data as MessageModel[];  // Messages from API response
        } else {
          console.error(response.message);
        }
      },
      (error: any) => {
        console.error('Error fetching enquiry messages', error);
      }
    );
  }

  // Fetch the latest message for the enquiry
  getLatestMessageByEnquiryId(enquiryId: number) {
    this.enquiryService.getLatestMessageByEnquiryId(enquiryId).subscribe(
      (response: ResultDetailModel<MessageModel>) => {
        if (response.isSuccess) {
          this.replyInfo = response.data;  // Latest message information
        } else {
          console.error(response.message);
        }
      },
      (error: any) => {
        console.error('Error fetching latest message', error);
      }
    );
  }

  // Submit the reply message
  submitReply() {
    if (!this.replyInfo || !this.enquiryId) {
      console.error('Cannot submit reply, no reply information or enquiry ID.');
      return;
    }

    const replyPayload: MessageModel = {
      messageId: 0, receiver: "", regarding: "", sender: "",
      enquiryId: this.enquiryId,
      messageContent: this.replyMessageContent,
      senderUserId: this.currentUserId,
      receiverUserId: this.replyInfo.senderUserId !== this.currentUserId ? this.replyInfo.senderUserId : this.replyInfo.receiverUserId,
      messageDate: new Date()  // Add the current date for the message
    };

    // Post the reply via the service
    this.enquiryService.createMessage(replyPayload).subscribe(
      (response: ResultDetailModel<boolean>) => {
        if (response.isSuccess) {
          // Handle successful reply submission, navigate back to enquiry list
          this.router.navigate(['/enquiry']);
        } else {
          console.error(response.message);
        }
      },
      (error: any) => {
        console.error('Error submitting reply', error);
      }
    );
  }
}
