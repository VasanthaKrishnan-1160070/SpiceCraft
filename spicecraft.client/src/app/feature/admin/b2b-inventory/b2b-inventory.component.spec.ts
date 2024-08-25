import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bInventoryComponent } from './b2b-inventory.component';

describe('B2bInventoryComponent', () => {
  let component: B2bInventoryComponent;
  let fixture: ComponentFixture<B2bInventoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bInventoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
