using ChustaSoft.Common.Helpers;
using ChustaSoft.Tools.Authorization.Models;


namespace ChustaSoft.Tools.Authorization.Helpers
{
    public class SessionMapper : IMapper<User, string, Session>
    {

        public Session MapFromSource(User user, string userToken)
        {
            var session = MapFromSource(user);

            session.Token = userToken;

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
