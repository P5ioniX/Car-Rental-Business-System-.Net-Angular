import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LastViewedVehiclesComponent } from './last-viewed-vehicles.component';

describe('LastViewedVehiclesComponent', () => {
  let component: LastViewedVehiclesComponent;
  let fixture: ComponentFixture<LastViewedVehiclesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LastViewedVehiclesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LastViewedVehiclesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
