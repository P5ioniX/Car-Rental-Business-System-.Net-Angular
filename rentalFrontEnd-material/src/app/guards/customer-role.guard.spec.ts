import { TestBed, async, inject } from '@angular/core/testing';

import { CustomerRoleGuard } from './customer-role.guard';

describe('CustomerRoleGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CustomerRoleGuard]
    });
  });

  it('should ...', inject([CustomerRoleGuard], (guard: CustomerRoleGuard) => {
    expect(guard).toBeTruthy();
  }));
});
