using System;

namespace ChustaSoft.Tools.Authorization.Models
{
    [Serializable]
    public class UserActivation
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public bool Activate { get; set; }

    }
}
