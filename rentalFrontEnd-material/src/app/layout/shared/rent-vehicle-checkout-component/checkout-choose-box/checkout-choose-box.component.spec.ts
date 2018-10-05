import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckoutChooseBoxComponent } from './checkout-choose-box.component';

describe('CheckoutChooseBoxComponent', () => {
  let component: CheckoutChooseBoxComponent;
  let fixture: ComponentFixture<CheckoutChooseBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckoutChooseBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckoutChooseBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
