import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bProfileComponent } from './b2b-profile.component';

describe('B2bProfileComponent', () => {
  let component: B2bProfileComponent;
  let fixture: ComponentFixture<B2bProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [B2bProfileComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
