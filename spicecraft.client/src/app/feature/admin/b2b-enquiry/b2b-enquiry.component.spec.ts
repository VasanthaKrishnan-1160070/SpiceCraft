import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bEnquiryComponent } from './b2b-enquiry.component';

describe('B2bEnquiryComponent', () => {
  let component: B2bEnquiryComponent;
  let fixture: ComponentFixture<B2bEnquiryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bEnquiryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bEnquiryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
