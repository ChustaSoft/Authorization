using Microsoft.EntityFrameworkCore;

namespace ChustaSoft.Tools.Authorization.TestCustom.WebAPI
{
    public class AuthCustomContext : AuthorizationContextBase<CustomUser, CustomRole>
    {

        public AuthCustomContext(DbContextOptions<AuthCustomContext> options)
           : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Custom tables or relations could be placed here

            base.OnModelCreating(modelBuilder);
        }

    }

}
