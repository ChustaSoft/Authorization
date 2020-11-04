import { LoginType, Session } from "..";

export class UserValidationHelper {

    public getConfirmationToken(loginType: LoginType, session: Session) : string {        
        switch (loginType) {
            case LoginType.MAIL:
                return session.parameters['EmailConfirmationToken'];
        
            case LoginType.PHONE:
                return session.parameters['PhoneConfirmationToken'];

            default:
                throw new Error('Invalid login type');
        }
    }

}