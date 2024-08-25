import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bCustomerComponent } from './b2b-customer.component';

describe('B2bCustomerComponent', () => {
  let component: B2bCustomerComponent;
  let fixture: ComponentFixture<B2bCustomerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bCustomerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
