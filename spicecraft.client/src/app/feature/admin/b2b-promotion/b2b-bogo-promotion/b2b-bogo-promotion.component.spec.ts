import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bBogoPromotionComponent } from './b2b-bogo-promotion.component';

describe('B2bBogoPromotionComponent', () => {
  let component: B2bBogoPromotionComponent;
  let fixture: ComponentFixture<B2bBogoPromotionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bBogoPromotionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bBogoPromotionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
