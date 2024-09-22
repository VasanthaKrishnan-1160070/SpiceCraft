import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bCategoryPromotionComponent } from './b2b-category-promotion.component';

describe('B2bCategoryPromotionComponent', () => {
  let component: B2bCategoryPromotionComponent;
  let fixture: ComponentFixture<B2bCategoryPromotionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bCategoryPromotionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bCategoryPromotionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
