import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffRegistrationComponent } from './staff-registration.component';

describe('StaffRegistrationComponent', () => {
  let component: StaffRegistrationComponent;
  let fixture: ComponentFixture<StaffRegistrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StaffRegistrationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StaffRegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
