import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bResetPasswordComponent } from './b2b-reset-password.component';

describe('B2bResetPasswordComponent', () => {
  let component: B2bResetPasswordComponent;
  let fixture: ComponentFixture<B2bResetPasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bResetPasswordComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bResetPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
