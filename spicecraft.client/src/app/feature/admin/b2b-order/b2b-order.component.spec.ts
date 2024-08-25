import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bOrderComponent } from './b2b-order.component';

describe('B2bOrderComponent', () => {
  let component: B2bOrderComponent;
  let fixture: ComponentFixture<B2bOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bOrderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
