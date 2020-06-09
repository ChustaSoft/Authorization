using System;

namespace ChustaSoft.Tools.Authorization
{
    public class TokenInfo
    {

        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }


        public TokenInfo(string token, DateTime expirationDate)
        {
            Token = token;
            ExpirationDate = expirationDate;
        }

    }
}
