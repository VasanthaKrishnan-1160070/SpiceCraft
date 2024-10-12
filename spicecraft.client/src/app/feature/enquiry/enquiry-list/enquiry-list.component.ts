import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {
  DxButtonModule,
  DxDataGridModule,
  DxFormModule,
  DxPopupModule,
  DxSelectBoxModule,
  DxTextBoxModule
} from "devextreme-angular";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ItemCardComponent} from "../../item/item-card/item-card.component";
import {AsyncPipe, JsonPipe, NgIf} from "@angular/common";
import {TitleComponent} from "../../../shared/components/title/title.component";
import {
  DxiColumnModule,
  DxoColumnChooserModule,
  DxoLoadPanelModule,
  DxoMasterDetailModule, DxoPagerModule, DxoPagingModule
} from "devextreme-angular/ui/nested";
import {UserService} from "../../../core/service/user.service";
import {Observable, Subject} from "rxjs";
import {UserModel} from "../../../core/model/user/user.model";
import {map, takeUntil} from "rxjs/operators";
import {EnquiryService} from "../../../core/service/enquiry.service";
import {EnquiryModel} from "../../../core/model/enquiry/enquiry-model";
import {Router, RouterLink} from "@angular/router";
import {AuthService} from "../../../core/service/auth.service";
import {InternalEnquiryModel} from "../../../core/model/enquiry/internal-enquiry.model";
import {EnquiryCustomerModel} from "../../../core/model/enquiry/enquiry-customer.model";

@Component({
    selector: 'sc-enquiry-list',
    templateUrl: './enquiry-list.component.html',
    styleUrl: './enquiry-list.component.css',
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
  private _enquiryService = inject(EnquiryService);
  private _userService = inject(UserService);
  private _destroy$: Subject<void> = new Subject<void>();
  private _authService = inject(AuthService);
  private _router = inject(Router);

  private currentUserId = 0;

  public enquiries$!: Observable<EnquiryModel[]>;
  public internalEnquiry$!: Observable<InternalEnquiryModel>;
  public customers$!: Observable<EnquiryCustomerModel[]>;

  ngOnInit() {
    const isInternalUser = this._authService.isInternalUser();
    this.currentUserId = this._userService.getCurrentUserId();

    if (isInternalUser) {
      this.internalEnquiry$ = this._enquiryService.getEnquiriesForInternalUser(this.currentUserId).pipe(
        map(m => m.data as InternalEnquiryModel),
        takeUntil(this._destroy$)
      );

      this.enquiries$ = this.internalEnquiry$.pipe( map(m => m.enquiries));
      // for selecting the customer by the internal users
      this.customers$ = this.internalEnquiry$.pipe( map(m => m.customers));
    }
    else {
      this.enquiries$ = this._enquiryService.getEnquiriesByUser(this.currentUserId).pipe(
        map(m => m.data as EnquiryModel[]),
        takeUntil(this._destroy$)
      )
    }
  }

  navigateToDetail(rowData: any){
    const enquiryId = rowData.data.enquiryId;
    this._router.navigate(['enquiry-details', enquiryId]);
  }

  ngOnDestroy() {
    this._destroy$.next();
  }
}
