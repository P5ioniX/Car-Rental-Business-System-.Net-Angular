import { TestBed, inject } from '@angular/core/testing';

import { UserServise } from './user.service';

describe('UserServiseService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UserServise]
    });
  });

  it('should be created', inject([UserServise], (service: UserServise) => {
    expect(service).toBeTruthy();
  }));
});
