import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerRentalsComponent } from './customer-rentals.component';

describe('CustomerRentalsComponent', () => {
  let component: CustomerRentalsComponent;
  let fixture: ComponentFixture<CustomerRentalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerRentalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerRentalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
