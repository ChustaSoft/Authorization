import { Credentials, Session, UserActivation, UserValidation } from "..";
import { HttpService } from '@chustasoft/cs-common'

export class AuthorizationService {

    private httpService: HttpService;
    private authorizationApiUrl: string;

    
    constructor(authorizationApiUrl: string) {
        this.authorizationApiUrl = authorizationApiUrl;

        this.httpService = new HttpService();
    }


    public async register(credentials: Credentials): Promise<Session> {
        return this.httpService.post<Credentials, Session>(
            `${this.authorizationApiUrl}auth/register`, credentials);
    }

    public async login(credentials: Credentials): Promise<Session> {
        return this.httpService.post<Credentials, Session>(
            `${this.authorizationApiUrl}auth/login`, credentials);
    }

    public async confirm(userValidation: UserValidation): Promise<Session> {
        return this.httpService.post<UserValidation, Session>(
            `${this.authorizationApiUrl}auth/confirm`, userValidation);
    }

    public async activate(userActivation: UserActivation): Promise<string> {
        return this.httpService.post<UserActivation, string>(
            `${this.authorizationApiUrl}auth/activate`, userActivation);
    }

}