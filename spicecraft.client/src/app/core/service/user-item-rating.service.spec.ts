import { TestBed } from '@angular/core/testing';

import { UserItemRatingService } from './user-item-rating.service';

describe('UserItemRatingService', () => {
  let service: UserItemRatingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserItemRatingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
