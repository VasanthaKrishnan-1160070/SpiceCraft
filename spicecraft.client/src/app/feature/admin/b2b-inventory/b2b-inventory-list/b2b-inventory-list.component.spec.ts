import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bInventoryListComponent } from './b2b-inventory-list.component';

describe('InventoryListComponent', () => {
  let component: B2bInventoryListComponent;
  let fixture: ComponentFixture<B2bInventoryListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bInventoryListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bInventoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
