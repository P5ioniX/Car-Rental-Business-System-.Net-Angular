import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RentSumCalculatorComponent } from './rent-sum-calculator.component';

describe('RentSumCalculatorComponent', () => {
  let component: RentSumCalculatorComponent;
  let fixture: ComponentFixture<RentSumCalculatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RentSumCalculatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RentSumCalculatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
