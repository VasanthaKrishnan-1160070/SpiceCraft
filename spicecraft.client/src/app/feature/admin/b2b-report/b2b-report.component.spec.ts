import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bReportComponent } from './b2b-report.component';

describe('B2bReportComponent', () => {
  let component: B2bReportComponent;
  let fixture: ComponentFixture<B2bReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bReportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(B2bReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
