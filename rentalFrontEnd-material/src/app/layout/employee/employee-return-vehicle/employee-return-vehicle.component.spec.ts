import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeReturnVehicleComponent } from './employee-return-vehicle.component';

describe('EmployeeReturnVehicleComponent', () => {
  let component: EmployeeReturnVehicleComponent;
  let fixture: ComponentFixture<EmployeeReturnVehicleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeReturnVehicleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeReturnVehicleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
