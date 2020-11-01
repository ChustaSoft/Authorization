import { Credentials, Session } from "..";
import { ActionResponse } from '@chustasoft/cs-common'

export class AuthorizationService {

    private authorizationApiUrl: string;

    
    constructor(authorizationApiUrl: string) {
        this.authorizationApiUrl = authorizationApiUrl;
    }


    public async register(credentials: Credentials): Promise<Session> {
        return fetch(`${this.authorizationApiUrl}/api/auth/register`, {
            method: 'POST',
            body: JSON.stringify(credentials),
            headers: {
                'Content-Type': 'application/json',
                'Accept': `application/json, text/plain, */*`
            }})
            .then(httpRespose => {
                if (!httpRespose.ok) {
                    throw new Error(httpRespose.statusText)
                }
                return httpRespose.json() as Promise<ActionResponse<Session>>;
            })
            .then(actionResponse => {
                return actionResponse.data
            });
    }

}