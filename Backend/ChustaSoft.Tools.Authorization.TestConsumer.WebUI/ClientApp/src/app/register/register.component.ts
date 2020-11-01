import { Component } from '@angular/core';
import { Credentials } from '@chustasoft/cs-authorization-connector';
import { AuthService } from '../auth.service';
import { Culture } from '../models/culture.interface';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  cultures: Culture[] = [
    { code: 'en-UK', text: 'English - United Kingdom' },
    { code: 'en-US', text: 'English - United States' },
    { code: 'es-ES', text: 'Spanish - Spain' },
    { code: 'fr-FR', text: 'French - France' }
  ];

  public credentials: Credentials = {
    username: '',
    email: '',
    password: '',
    phone: '',
    culture: '',
    parameters: {}
  };

  constructor(private authService: AuthService) { }

  public register(): void {
    this.authService.register(this.credentials)
      .then(x =>
      {
        //THEN: Session object is retrived here, allowing to manage user token

        return x;
      });
  }

}
