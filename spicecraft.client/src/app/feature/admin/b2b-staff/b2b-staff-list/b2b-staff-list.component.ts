import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {TitleComponent} from "../../../../shared/components/title/title.component";
import {DxButtonModule, DxDataGridModule} from "devextreme-angular";
import {UserService} from "../../../../core/service/user.service";
import {Observable, Subject} from "rxjs";
import {take, takeUntil} from "rxjs/operators";
import {UserModel} from "../../../../core/model/user/user.model";
import {AsyncPipe, JsonPipe, NgIf} from "@angular/common";
import {RouterLink} from "@angular/router";
import {NotifyService} from "../../../../core/service/notify.service";

@Component({
  selector: 'sc-b2b-staff-list',
  templateUrl: './b2b-staff-list.component.html',
  styleUrl: './b2b-staff-list.component.css',
  imports: [
    TitleComponent,
    DxDataGridModule,
    AsyncPipe,
    NgIf,
    JsonPipe,
    DxButtonModule,
    RouterLink
  ],
  standalone: true
})
export class B2bStaffListComponent implements OnInit, OnDestroy {
  private _userService = inject(UserService);
  private _destroy$: Subject<void> = new Subject<void>();
  private _notifyService = inject(NotifyService);

  public staffs$!: Observable<UserModel[]>;

  ngOnInit() {
     this.loadStaffs();
  }

  toggelUserActive(userId: number) {
    this._userService.toggleUserActive(userId).pipe(
      takeUntil(this._destroy$)
    )
      .subscribe(() => {
        this._notifyService.showSuccess('User Status is Updated successfully.');
        this.loadStaffs();
      });
  }

  loadStaffs() {
    this.staffs$ = this._userService.getStaffs().pipe(
      take(1),
      takeUntil(this._destroy$)
    )
  }

  ngOnDestroy() {
    this._destroy$.next();
  }

}
