import { Injectable, Inject } from '@angular/core';
import { AuthorizationService } from '@chustasoft/cs-authorization-connector';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends AuthorizationService {

  constructor(@Inject('BASE_URL') baseUrl: string)
  {    
    super(baseUrl);
  }

}
