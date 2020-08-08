using ChustaSoft.Common.Contracts;
using ChustaSoft.Tools.Authorization.Models;

namespace ChustaSoft.Tools.Authorization
{
    public class SessionMapper<TUser> : IMapper<TUser, TokenInfo, Session>
         where TUser : User, new()
    {

        public Session MapFromSource(TUser user, TokenInfo tokenInfo)
        {
            var session = MapFromSource(user);

            session.Token = tokenInfo.Token;
            session.ExpirationDate = tokenInfo.ExpirationDate;

            return session;
        }

        public Session MapFromSource(TUser user)
            => new Session
                {
                    Culture = user.Culture,
                    UserId = user.Id,
                    Username = user.UserName
                };

        public TUser MapToSource(Session session)
            => new TUser
            {
                    Culture = session.Culture,
                    Id = session.UserId,
                    UserName = session.Username
                };

    }



    public class SessionMapper : SessionMapper<User> , IMapper<User, TokenInfo, Session> { }

}
