using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization.Models
{
    [Serializable]
    public class Session
    {

        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Culture { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, string> Parameters { get; set; }
        

        public Session() { }

        public Session(User user, TokenInfo tokenInfo)
        {
            UserId = user.Id;
            Username = user.UserName;
            Culture = user.Culture;
            Token = tokenInfo.Token;
            ExpirationDate = tokenInfo.ExpirationDate;
        }

        public Session(User user, TokenInfo tokenInfo, IDictionary<string, string> parameters)
            : this(user, tokenInfo)
        {
            this.Parameters = parameters;
        }

    }
}
