import { Component, OnInit } from '@angular/core';
import { Credentials } from '@chustasoft/cs-authorization-connector';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public credentials: Credentials = {
    username: '',
    email: '',
    password: '',
    phone: '',
    culture: '',
    parameters: {}
  };


  constructor(private authService: AuthService) { }


  ngOnInit(): void {
  }


  public login(): void {
    this.authService.login(this.credentials)
      .then(x => {
        //THEN: Session object is retrived here, allowing to manage user token
        
        return x;
      });
  }
}
