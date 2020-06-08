using System;

namespace ChustaSoft.Tools.Authorization
{
    public class Session
    {

        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Culture { get; set; }

    }
}
