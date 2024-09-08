import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bStaffListComponent } from './b2b-staff-list.component';

describe('B2bStaffListComponent', () => {
  let component: B2bStaffListComponent;
  let fixture: ComponentFixture<B2bStaffListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [B2bStaffListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bStaffListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
