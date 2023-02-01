using ChustaSoft.Tools.Authorization.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public class AuthUserClaimsFactory<TUser> : UserClaimsPrincipalFactory<TUser>
        where TUser : User, new()
    {
        
        private readonly UserManager<TUser> _userManager;


        public AuthUserClaimsFactory(UserManager<TUser> userManager, IOptions<IdentityOptions> optionsAccessor) 
            : base(userManager, optionsAccessor)
        {
            _userManager = userManager;
        }


        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            identity.AddClaims(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));

            return identity;
        }
    }
}
