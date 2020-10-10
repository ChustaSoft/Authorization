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

    }
}
