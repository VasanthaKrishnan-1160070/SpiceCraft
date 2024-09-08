import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bCustomerListComponent } from './b2b-customer-list.component';

describe('B2bCustomerListComponent', () => {
  let component: B2bCustomerListComponent;
  let fixture: ComponentFixture<B2bCustomerListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bCustomerListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bCustomerListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
