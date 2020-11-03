import { Component, OnInit } from '@angular/core';
import { Credentials, Session, UserActivation } from '@chustasoft/cs-authorization-connector';
import { AuthService } from '../auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  private activateActionFlag: boolean = false;
  private userNameRetrived: string;

  public credentials: Credentials = {
    username: '',
    email: '',
    password: '',
    phone: '',
    culture: '',
    parameters: {}
  };

  public session: Session;


  constructor(
    private authService: AuthService,
    private snackBar: MatSnackBar)
  { }


  ngOnInit(): void
  { }


  public login(): void {
    this.authService.login(this.credentials)
      .then(x => {
        this.session = x;
        this.snackBar.openFromComponent(LoginCompleteSnackbar, {
          duration: 5000,
        });
      });
  }

  public activate(): void {
    const activationInfo: UserActivation = {
      'username': this.session.username,
      'password': this.credentials.password,
      'activate': this.activateActionFlag
    };
    
    this.authService.activate(activationInfo)
      .then(x => {
        this.userNameRetrived = x;
        this.activateActionFlag = !this.activateActionFlag;
      });
  }

}


@Component({
  selector: 'app-snackbar',
  template: `
  <span class="example-pizza-party">
    Login completed successfully
  </span>
  `  
})
export class LoginCompleteSnackbar { }
