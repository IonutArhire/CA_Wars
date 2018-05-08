import { TestBed, inject } from '@angular/core/testing';

import { MatchUIService } from './match-ui.service';

describe('MatchUIService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MatchUIService]
    });
  });

  it('should be created', inject([MatchUIService], (service: MatchUIService) => {
    expect(service).toBeTruthy();
  }));
});
