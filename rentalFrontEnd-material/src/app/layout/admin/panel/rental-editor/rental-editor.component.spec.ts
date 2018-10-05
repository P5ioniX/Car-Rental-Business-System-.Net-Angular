import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RentalEditorComponent } from './rental-editor.component';

describe('RentalEditorComponent', () => {
  let component: RentalEditorComponent;
  let fixture: ComponentFixture<RentalEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RentalEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RentalEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
