﻿namespace ChustaSoft.Tools.Authorization
{
    public interface ICredentialsBusiness
    {

        LoginType ValidateCredentials(Credentials credentials);

    }
}