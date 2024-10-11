import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {UserService} from "../../../core/service/user.service";
import {Observable, Subject} from "rxjs";
import {UserModel} from "../../../core/model/user/user.model";
import {take, takeUntil} from "rxjs/operators";
import {AsyncPipe, JsonPipe, NgIf} from "@angular/common";
import {DxButtonModule, DxDataGridModule, DxTemplateModule} from "devextreme-angular";
import {
  DxiColumnModule,
  DxoColumnChooserModule,
  DxoLoadPanelModule,
  DxoMasterDetailModule, DxoPagerModule, DxoPagingModule
} from "devextreme-angular/ui/nested";
import {RouterLink} from "@angular/router";
import {NotifyService} from "../../../core/service/notify.service";

@Component({
  selector: 'sc-b2b-dealers',
  standalone: true,
  imports: [
    TitleComponent,
    AsyncPipe,
    DxDataGridModule,
    DxTemplateModule,
    DxiColumnModule,
    DxoColumnChooserModule,
    DxoLoadPanelModule,
    DxoMasterDetailModule,
    DxoPagerModule,
    DxoPagingModule,
    JsonPipe,
    NgIf,
    DxButtonModule,
    RouterLink
  ],
  templateUrl: './b2b-dealers.component.html',
  styleUrl: './b2b-dealers.component.css'
})
export class B2bDealersComponent implements OnInit, OnDestroy {
  private _userService = inject(UserService);
  private _destroy$: Subject<void> = new Subject<void>();
  private _notifyService = inject(NotifyService);

  public dealers$!: Observable<UserModel[]>;
  ngOnInit() {
    this.loadDealers();
  }

  toggelUserActive(userId: number) {
    this._userService.toggleUserActive(userId).pipe(
      takeUntil(this._destroy$)
    )
      .subscribe(() => {
        this._notifyService.showSuccess('User Status is Updated successfully.');
        this.loadDealers();
      });
  }

  loadDealers() {
    this.dealers$ = this._userService.getDealers().pipe(
      take(1),
      takeUntil(this._destroy$)
    )
  }

  ngOnDestroy() {
    this._destroy$.next();
  }
}
