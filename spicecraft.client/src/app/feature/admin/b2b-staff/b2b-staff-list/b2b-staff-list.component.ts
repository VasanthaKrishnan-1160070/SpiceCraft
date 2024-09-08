import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {TitleComponent} from "../../../../shared/components/title/title.component";
import {DxDataGridModule} from "devextreme-angular";
import {UserService} from "../../../../core/service/user.service";
import {Observable, Subject} from "rxjs";
import {takeUntil} from "rxjs/operators";
import {UserModel} from "../../../../core/model/user/user.model";
import {AsyncPipe, JsonPipe, NgIf} from "@angular/common";

@Component({
  selector: 'sc-b2b-staff-list',
  templateUrl: './b2b-staff-list.component.html',
  styleUrl: './b2b-staff-list.component.css',
  imports: [
    TitleComponent,
    DxDataGridModule,
    AsyncPipe,
    NgIf,
    JsonPipe
  ],
  standalone: true
})
export class B2bStaffListComponent implements OnInit, OnDestroy {
  private _userService = inject(UserService);
  private _destroy$: Subject<void> = new Subject<void>();

  public staffs$!: Observable<UserModel[]>;
  ngOnInit() {
     this.staffs$ = this._userService.getStaffs().pipe(
       takeUntil(this._destroy$)
     )
  }

  ngOnDestroy() {
    this._destroy$.next();
  }

}
