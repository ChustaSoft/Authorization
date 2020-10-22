import { TestBed } from '@angular/core/testing';
import { AuthorizationService } from './authorization-connector.service';

describe('AuthorizationConnectorService', () => {
  let service: AuthorizationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthorizationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
