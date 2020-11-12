using System;

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
        

        public Session() { }

        public Session(User user, TokenInfo tokenInfo)
        {
            UserId = user.Id;
            Username = user.UserName;
            Culture = user.Culture;
            Token = tokenInfo.Token;
            ExpirationDate = tokenInfo.ExpirationDate;
        }

    }
}
