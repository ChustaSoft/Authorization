using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization
{
    public class UserEventArgs : EventArgs
    {
        public Guid UserId { get; private set; }
        public IDictionary<string, string> Parameters { get; private set; }


        public UserEventArgs(Guid userId) 
            : this(userId, new Dictionary<string, string>()) 
        { }        

        public UserEventArgs(Guid userId, IDictionary<string, string> parameters)
            : base()
        {
            UserId = userId;
            Parameters = parameters;
        }

    }
}
