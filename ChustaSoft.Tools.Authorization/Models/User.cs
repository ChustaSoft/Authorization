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
        public bool Confirmed { get; set; }
        public override bool EmailConfirmed 
        {
            get { return Confirmed; }
            set { Confirmed = value; } 
        }
        public override bool PhoneNumberConfirmed
        {
            get { return Confirmed; }
            set { Confirmed = value; }
        }

        public string Culture { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }

    }
}
