using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChustaSoft.Tools.Authorization.Models
{
    [Serializable]
    public class Credentials
    {

        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Culture { get; set; }

        public IDictionary<string, string> Parameters { get; set; }


        public Credentials()
        {
            Parameters = new Dictionary<string, string>();
        }

    }
}
