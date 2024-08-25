import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bDealersComponent } from './b2b-dealers.component';

describe('B2bDealersComponent', () => {
  let component: B2bDealersComponent;
  let fixture: ComponentFixture<B2bDealersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bDealersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bDealersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
