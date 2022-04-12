using ChustaSoft.Tools.Authorization.Models;
using Microsoft.EntityFrameworkCore;

namespace ChustaSoft.Tools.Authorization
{
    public class AuthIdentityContext : AuthorizationContextBase<User, Role>
    {

        public AuthIdentityContext(DbContextOptions<AuthIdentityContext> options)
           : base(options)
        { }

    }

}
