import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Credentials } from '../public-api';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  constructor(private httpClient: HttpClient) { }

  public register(credentials: Credentials) : Promise<Session>{
    debugger;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': `application/json, text/plain, */*`
    });

    return this.httpClient.post('https://localhost:44374/api/auth/register', credentials, { headers: headers })      
      .toPromise();
  }

}

//Crear Session
//Crear ActionResult
//Devolver sólo el session, Promise.all[lllamanda].then([actionResult] => { return actionResult.data})