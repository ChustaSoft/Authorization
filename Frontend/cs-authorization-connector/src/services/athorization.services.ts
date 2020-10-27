import { Credentials, Session } from "..";
import { ActionResponse } from '@chustasoft/cs-common'


export class AuthorizationService {

    public async register(credentials: Credentials): Promise<Session> {
        return new Promise<ActionResponse<Session>>(async (resolve, reject) => {
            return await fetch('https://localhost:44374/api/auth/register', {
                method: 'POST',
                body: JSON.stringify(credentials),
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': `application/json, text/plain, */*`
                }
            });
        })
            .then(x => x.data);
    }

}