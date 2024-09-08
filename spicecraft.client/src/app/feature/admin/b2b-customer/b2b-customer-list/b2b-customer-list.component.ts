import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {UserService} from "../../../../core/service/user.service";
import {Observable, Subject} from "rxjs";
import {UserModel} from "../../../../core/model/user/user.model";
import {takeUntil} from "rxjs/operators";
import {AsyncPipe, JsonPipe, NgIf} from "@angular/common";
import {DxDataGridModule, DxTemplateModule} from "devextreme-angular";
import {
  DxiColumnModule,
  DxoColumnChooserModule,
  DxoLoadPanelModule,
  DxoMasterDetailModule
} from "devextreme-angular/ui/nested";
import {TitleComponent} from "../../../../shared/components/title/title.component";

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
    TitleComponent
  ],
  templateUrl: './b2b-customer-list.component.html',
  styleUrl: './b2b-customer-list.component.css'
})
export class B2bCustomerListComponent implements OnInit, OnDestroy {
  private _userService = inject(UserService);
  private _destroy$: Subject<void> = new Subject<void>();

  public customers$!: Observable<UserModel[]>;
  ngOnInit() {
    this.customers$ = this._userService.getCustomers().pipe(
      takeUntil(this._destroy$)
    )
  }

  ngOnDestroy() {
    this._destroy$.next();
  }

}
