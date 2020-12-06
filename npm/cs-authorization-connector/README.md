# cs-authorization-connector


## Frontend tool

ChustaSoft Authorization provides a npm package (_**@chustasoft/cs-authorization-connector**_) that allow a frontend to connect with the exposed Controller actions of the AspNet package. 

This package is developed purely in TypeScript, so the package can easily integrated in any kind of JavaScript project.

The package provides:
- AuthorizationService: Connection service with the WebApi
  - _async register(credentials: Credentials): Promise<Session>_
  - _async login(credentials: Credentials): Promise<Session>_
  - _async confirm(userValidation: UserValidation): Promise<Session>_
  - _async activate(userActivation: UserActivation): Promise<string>_


## Example on Angular application

1. Install @chustasoft/cs-authorization-connector package

2. Recommended usage of Service with Angular injection:

```javascript

import { Injectable, Inject } from '@angular/core';
import { AuthorizationService } from '@chustasoft/cs-authorization-connector';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends AuthorizationService {

  //Could it be environment file inestad
  constructor(@Inject('BASE_URL') baseUrl: string)
  {    
    super(baseUrl);
  }

}

```
_With this approach, we will be both injecting the necessary configuration of the base URL of the API to the connector service, and adding the service to the Angular DI Container, therefore available for any component._



You will be able to found a complete example with Backend + Frontend, using Nuget packages and npm connector [here](https://github.com/ChustaSoft/Authorization/tree/master/Examples/ChustaSoft.Tools.Authorization.TestConsumer.WebUI)