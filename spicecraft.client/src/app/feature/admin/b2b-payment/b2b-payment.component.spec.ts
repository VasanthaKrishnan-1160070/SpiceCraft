import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bPaymentComponent } from './b2b-payment.component';

describe('B2bPaymentComponent', () => {
  let component: B2bPaymentComponent;
  let fixture: ComponentFixture<B2bPaymentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bPaymentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
