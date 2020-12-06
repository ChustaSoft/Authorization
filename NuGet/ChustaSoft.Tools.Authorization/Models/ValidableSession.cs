using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization.Models
{
    [Serializable]
    public class ValidableSession : Session
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, string> Parameters { get; set; }


        public ValidableSession(User user, TokenInfo tokenInfo, IDictionary<string, string> parameters)
            : base(user, tokenInfo)
        {
            this.Parameters = parameters;
        }

    }
}
