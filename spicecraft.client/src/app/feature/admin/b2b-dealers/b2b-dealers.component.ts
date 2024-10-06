import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {TitleComponent} from "../../../shared/components/title/title.component";
import {UserService} from "../../../core/service/user.service";
import {Observable, Subject} from "rxjs";
import {UserModel} from "../../../core/model/user/user.model";
import {takeUntil} from "rxjs/operators";
import {AsyncPipe, JsonPipe, NgIf} from "@angular/common";
import {DxButtonModule, DxDataGridModule, DxTemplateModule} from "devextreme-angular";
import {
  DxiColumnModule,
  DxoColumnChooserModule,
  DxoLoadPanelModule,
  DxoMasterDetailModule, DxoPagerModule, DxoPagingModule
} from "devextreme-angular/ui/nested";
import {RouterLink} from "@angular/router";

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

  public dealers$!: Observable<UserModel[]>;
  ngOnInit() {
    this.dealers$ = this._userService.getDealers().pipe(
      takeUntil(this._destroy$)
    )
  }
  ngOnDestroy() {
    this._destroy$.next();
  }
}
