import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RentalResultsComponent } from './rental-results.component';

describe('RentalResultsComponent', () => {
  let component: RentalResultsComponent;
  let fixture: ComponentFixture<RentalResultsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RentalResultsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RentalResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
