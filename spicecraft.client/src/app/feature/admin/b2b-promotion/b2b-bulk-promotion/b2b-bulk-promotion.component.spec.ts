import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bBulkPromotionComponent } from './b2b-bulk-promotion.component';

describe('B2bBulkPromotionComponent', () => {
  let component: B2bBulkPromotionComponent;
  let fixture: ComponentFixture<B2bBulkPromotionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bBulkPromotionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bBulkPromotionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
