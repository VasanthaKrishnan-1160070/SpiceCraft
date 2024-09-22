import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bPromotionComponent } from './b2b-promotion.component';

describe('B2bPromotionComponent', () => {
  let component: B2bPromotionComponent;
  let fixture: ComponentFixture<B2bPromotionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bPromotionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bPromotionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
