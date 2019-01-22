using ChustaSoft.Common.Helpers;
using ChustaSoft.Tools.Authorization.Models;


namespace ChustaSoft.Tools.Authorization.Helpers
{
    public class SessionMapper : IMapper<User, TokenInfo, Session>
    {

        public Session MapFromSource(User user, TokenInfo tokenInfo)
        {
            var session = MapFromSource(user);

            session.Token = tokenInfo.Token;
            session.ExpirationDate = tokenInfo.ExpirationDate;

            return session;
        }

        public Session MapFromSource(User user)
            => new Session
                {
                    Culture = user.Culture,
                    UserId = user.Id,
                    Username = user.UserName
                };

        public User MapToSource(Session session)
            => new User
                {
                    Culture = session.Culture,
                    Id = session.UserId,
                    UserName = session.Username
                };

    }
}
