import { TestBed, async, inject } from '@angular/core/testing';

import { EmployeeRoleGuard } from './employee-role.guard';

describe('EmployeeRoleGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EmployeeRoleGuard]
    });
  });

  it('should ...', inject([EmployeeRoleGuard], (guard: EmployeeRoleGuard) => {
    expect(guard).toBeTruthy();
  }));
});
