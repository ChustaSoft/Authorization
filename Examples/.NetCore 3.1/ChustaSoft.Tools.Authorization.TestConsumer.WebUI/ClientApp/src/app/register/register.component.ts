import { Component } from '@angular/core';
import { Credentials, LoginType, Session, UserValidation, UserValidationHelper } from '@chustasoft/cs-authorization-connector';
import { AuthService } from '../auth.service';
import { Culture } from '../models/culture.interface';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  LoginType: typeof LoginType = LoginType;

  public cultures: Culture[] = [
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


  public session: Session = { } as Session;

  public hasEmailConfirmation() {
    return this.session !== undefined && this.session.parameters['EmailConfirmationToken'] !== undefined;
  }
    
  public hasPhoneConfirmation() {
    return this.session !== undefined && this.session.parameters['PhoneConfirmationToken'] !== undefined;;
  };
  
  private userValidationHelper: UserValidationHelper;


  constructor(private authService: AuthService)
  {
    this.userValidationHelper = new UserValidationHelper();
  }

  public register(): void {
    this.authService.register(this.credentials)
      .then(x =>
      {
        this.session = x;
      });
  }

  public confirm(loginType: LoginType): void {
    const confirmationInfo: UserValidation = {
      'email': (loginType === LoginType.MAIL) ? this.credentials.email : '',
      'phone': (loginType === LoginType.PHONE) ? this.credentials.phone: '',
      'confirmationToken': this.userValidationHelper.getConfirmationToken(loginType, this.session)
    };

    this.authService.confirm(confirmationInfo)
      .then(x => {
        //TODO: Another session is retrived here, done what is required.
      });
  }

}
