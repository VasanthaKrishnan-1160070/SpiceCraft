import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bStaffComponent } from './b2b-staff.component';

describe('B2bStaffComponent', () => {
  let component: B2bStaffComponent;
  let fixture: ComponentFixture<B2bStaffComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bStaffComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bStaffComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
