import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bUserComponent } from './b2b-user.component';

describe('B2bUserComponent', () => {
  let component: B2bUserComponent;
  let fixture: ComponentFixture<B2bUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bUserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
