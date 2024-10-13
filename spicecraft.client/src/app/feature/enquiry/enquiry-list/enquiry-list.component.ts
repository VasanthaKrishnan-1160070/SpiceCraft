import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import {
  DxButtonModule,
  DxDataGridModule,
  DxFormModule,
  DxPopupModule,
  DxSelectBoxModule,
  DxTextBoxModule
} from 'devextreme-angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ItemCardComponent } from '../../item/item-card/item-card.component';
import { AsyncPipe, JsonPipe, NgIf } from '@angular/common';
import { TitleComponent } from '../../../shared/components/title/title.component';
import {
  DxiColumnModule,
  DxoColumnChooserModule,
  DxoLoadPanelModule,
  DxoMasterDetailModule,
  DxoPagerModule,
  DxoPagingModule
} from 'devextreme-angular/ui/nested';
import { UserService } from '../../../core/service/user.service';
import { Observable, Subject } from 'rxjs';
import {map, take, takeUntil} from 'rxjs/operators';
import { EnquiryService } from '../../../core/service/enquiry.service';
import { EnquiryModel } from '../../../core/model/enquiry/enquiry-model';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/service/auth.service';
import { InternalEnquiryModel } from '../../../core/model/enquiry/internal-enquiry.model';
import { EnquiryCustomerModel } from '../../../core/model/enquiry/enquiry-customer.model';
import {EnquiryTypeModel} from "../../../core/model/enquiry/enquiry-types";
import {NotifyService} from "../../../core/service/notify.service";

@Component({
  selector: 'sc-enquiry-list',
  templateUrl: './enquiry-list.component.html',
  styleUrls: ['./enquiry-list.component.css'],
  imports: [
    DxSelectBoxModule,
    DxTextBoxModule,
    FormsModule,
    ItemCardComponent,
    NgIf,
    ReactiveFormsModule,
    TitleComponent,
    AsyncPipe,
    DxDataGridModule,
    DxiColumnModule,
    DxoColumnChooserModule,
    DxoLoadPanelModule,
    DxoMasterDetailModule,
    DxoPagerModule,
    DxoPagingModule,
    JsonPipe,
    RouterLink,
    DxButtonModule,
    DxPopupModule,
    DxFormModule
  ],
  standalone: true
})
export class EnquiryListComponent implements OnInit, OnDestroy {
  private enquiryService = inject(EnquiryService);
  private userService = inject(UserService);
  private destroy$ = new Subject<void>();
  private authService = inject(AuthService);
  private router = inject(Router);
  private _notifyService = inject(NotifyService);

  public currentUserId = 0;

  public enquiries$!: Observable<EnquiryModel[]>;
  public internalEnquiry$!: Observable<InternalEnquiryModel>;
  public customers$!: Observable<EnquiryCustomerModel[]>;

  // Added properties for the modal and form
  public isModalVisible = false;
  public formData: any = {};
  public isDisabled = false;
  public isUserInternal = false;
  public enquiryTypeDataSource: any[] = [];
  private formInstance: any;

  ngOnInit() {
    this.isUserInternal = this.authService.isInternalUser();
    this.currentUserId = this.userService.getCurrentUserId();

    if (this.isUserInternal) {
      this.internalEnquiry$ = this.enquiryService.getEnquiriesForInternalUser(this.currentUserId).pipe(
        map((m) => m.data as InternalEnquiryModel),
        takeUntil(this.destroy$)
      );

      this.enquiries$ = this.internalEnquiry$.pipe(map((m) => m.enquiries));
      // For selecting the customer by the internal users
      this.customers$ = this.internalEnquiry$.pipe(map((m) => m.customers));
    } else {
      this.enquiries$ = this.enquiryService.getEnquiriesByUser(this.currentUserId).pipe(
        map((m) => m.data as EnquiryModel[]),
        takeUntil(this.destroy$)
      );
    }

    // Load data for Enquiry Types
    this.loadEnquiryTypes();
  }

  /** Load Enquiry Types for the SelectBox */
  loadEnquiryTypes() {
    this.enquiryService.getEnquiryTypes().pipe(
      map(result =>  result.data as EnquiryTypeModel[] ),
      take(1),
      takeUntil(this.destroy$)
    ).subscribe(s => {
      this.enquiryTypeDataSource = s;
    });
  }

  /** Get Current User ID */
  getCurrentUserId() {
    return this.userService.getCurrentUserId();
  }

  /** Open Modal and Initialize Form Data */
  openModal() {
    this.isModalVisible = true;

    // Initialize formData
    this.formData = {
      enquiryTypeId: null,
      receiverUserId: null,
      initialMessage: '',
      senderUserId: this.getCurrentUserId(),
    };
  }

  /** Capture the form instance on initialization */
  onFormInitialized(e: any) {
    this.formInstance = e.component;
  }


  /** Handle Form Submission */
  onFormSubmit() {
    const result = this.formInstance.validate();

    if (result.isValid) {
      this.submitForm(this.formData);
      this.isModalVisible = false;
    } else {
      this._notifyService.showError("Please fix the validation error and try again");
    }
  }

  /** Submit Form Data to Backend */
  submitForm(formData: any) {
    this.enquiryService.createEnquiry(formData).subscribe(
      (response) => {
        // Handle successful submission
        console.log('Enquiry submitted successfully');
      },
      (error) => {
        // Handle submission error
        console.error('Error submitting enquiry:', error);
      }
    );
  }

  /** Navigate to Enquiry Detail Page */
  navigateToDetail(rowData: any) {
    const enquiryId = rowData.data.enquiryId;
    this.router.navigate(['enquiry-details', enquiryId]);
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
