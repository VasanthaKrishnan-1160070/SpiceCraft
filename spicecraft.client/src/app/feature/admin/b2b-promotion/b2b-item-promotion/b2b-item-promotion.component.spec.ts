import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bItemPromotionComponent } from './b2b-item-promotion.component';

describe('B2bItemPromotionComponent', () => {
  let component: B2bItemPromotionComponent;
  let fixture: ComponentFixture<B2bItemPromotionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [B2bItemPromotionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bItemPromotionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
