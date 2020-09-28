using Microsoft.AspNetCore.Identity;
using System;

namespace ChustaSoft.Tools.Authorization.Models
{
    public class User : IdentityUser<Guid>
    {
        public override Guid Id { get; set; }
        public override string UserName { get; set; }
        public override string Email { get; set; }
        public override string PhoneNumber { get; set; }
        public string Culture { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
        public bool IsActive { get; set; }


        public bool HasValidEmail
            => !string.IsNullOrWhiteSpace(Email) && !Email.EndsWith(AuthorizationConstants.NO_EMAIL_SUFFIX_FORMAT);

        public bool HasValidPhone
            => !string.IsNullOrWhiteSpace(PhoneNumber);

        public Session GetSession(TokenInfo tokenInfo) 
        { 
            return new Session
            {
                UserId = Id,
                Username = UserName,
                Culture = Culture,
                Token = tokenInfo.Token,
                ExpirationDate = tokenInfo.ExpirationDate
            };
        }

    }
}
