import { Component } from '@angular/core';
import { AuthorizationService, Credentials } from 'projects/chustasoft/ngx-authorization-connector/src/public-api';
import { Culture } from '../culture.interface';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {

  cultures: Culture[] = [
    { code: 'en-UK', text: 'English - United Kingdom'},
    { code: 'en-US', text: 'English - United States'},
    { code: 'es-ES', text: 'Spanish - Spain'},
    { code: 'fr-FR', text: 'French - France'}
  ];

  public credentials: Credentials = <Credentials>({
    username: '',
    email: '',
    password: '',
    phone: '',
    culture: '',
    parameters: { }
  });

  constructor(private authService: AuthorizationService) { }

  public register(): void {
    this.authService.register(this.credentials)
    .then((respose) => {
      debugger;
    });
  }

}
