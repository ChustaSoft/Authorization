using ChustaSoft.Tools.Authorization.Models;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization
{
    public class UserEventArgs : EventArgs
    {
        public Guid UserId { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public IDictionary<string, string> Parameters { get; private set; }


        public UserEventArgs(User user) 
            : this(user, new Dictionary<string, string>()) 
        { }        

        public UserEventArgs(User user, IDictionary<string, string> parameters)
            : base()
        {
            UserId = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Parameters = parameters;
        }

    }
}
