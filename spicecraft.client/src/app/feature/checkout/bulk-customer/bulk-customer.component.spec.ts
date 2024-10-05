import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BulkCustomerComponent } from './bulk-customer.component';

describe('BulkCustomerComponent', () => {
  let component: BulkCustomerComponent;
  let fixture: ComponentFixture<BulkCustomerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BulkCustomerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BulkCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
