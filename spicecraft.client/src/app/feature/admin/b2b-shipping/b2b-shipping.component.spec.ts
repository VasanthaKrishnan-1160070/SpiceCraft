import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bShippingComponent } from './b2b-shipping.component';

describe('B2bShippingComponent', () => {
  let component: B2bShippingComponent;
  let fixture: ComponentFixture<B2bShippingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bShippingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bShippingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
