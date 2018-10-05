import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RentVehicleCheckoutComponentComponent } from './rent-vehicle-checkout-component.component';

describe('RentVehicleCheckoutComponentComponent', () => {
  let component: RentVehicleCheckoutComponentComponent;
  let fixture: ComponentFixture<RentVehicleCheckoutComponentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RentVehicleCheckoutComponentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RentVehicleCheckoutComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
