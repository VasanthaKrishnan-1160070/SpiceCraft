import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {UserService} from "../../../../core/service/user.service";
import {Observable, Subject} from "rxjs";
import {UserModel} from "../../../../core/model/user/user.model";
import {take, takeUntil} from "rxjs/operators";
import {AsyncPipe, JsonPipe, NgIf} from "@angular/common";
import {DxButtonModule, DxDataGridModule, DxTemplateModule} from "devextreme-angular";
import {
  DxiColumnModule,
  DxoColumnChooserModule,
  DxoLoadPanelModule,
  DxoMasterDetailModule
} from "devextreme-angular/ui/nested";
import {TitleComponent} from "../../../../shared/components/title/title.component";
import {RouterLink} from "@angular/router";
import {NotifyService} from "../../../../core/service/notify.service";

@Component({
  selector: 'sc-b2b-customer-list',
  standalone: true,
  imports: [
    AsyncPipe,
    DxDataGridModule,
    DxTemplateModule,
    DxiColumnModule,
    DxoColumnChooserModule,
    DxoLoadPanelModule,
    DxoMasterDetailModule,
    JsonPipe,
    NgIf,
    TitleComponent,
    DxButtonModule,
    RouterLink
  ],
  templateUrl: './b2b-customer-list.component.html',
  styleUrl: './b2b-customer-list.component.css'
})
export class B2bCustomerListComponent implements OnInit, OnDestroy {
  private _userService = inject(UserService);
  private _destroy$: Subject<void> = new Subject<void>();
  private _notifyService = inject(NotifyService)

  public customers$!: Observable<UserModel[]>;

  ngOnInit() {
    this.loadCustomers();
  }

  toggelUserActive(userId: number) {
    this._userService.toggleUserActive(userId).pipe(
      takeUntil(this._destroy$)
    )
      .subscribe(() => {
        this._notifyService.showSuccess('User Status is Updated successfully.');
        this.loadCustomers();
      });
  }

  loadCustomers() {
    this.customers$ = this._userService.getCustomers().pipe(
      take(1),
      takeUntil(this._destroy$)
    )
  }

  ngOnDestroy() {
    this._destroy$.next();
  }
}
