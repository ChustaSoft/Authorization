using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization.Models
{
    [Serializable]
    public class ValidableCredentials : Credentials
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, string> Parameters { get; set; }


        public ValidableCredentials()
        {
            Parameters = new Dictionary<string, string>();
        }

    }


    public class AutomaticCredentials : ValidableCredentials 
    {

        public bool FullAccess { get; set; }


        public AutomaticCredentials() : base() { }

    }

}
