using ChustaSoft.Common.Contracts;
using ChustaSoft.Tools.Authorization.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace ChustaSoft.Tools.Authorization.Helpers
{
    public class ClaimsIdentityBuilder<TUser> : IBuilder<Claim[]>
        where TUser : User, new()
    {

        private ICollection<Claim> _claims;


        internal ClaimsIdentityBuilder(TUser user)
        {
            _claims = new List<Claim> { 
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), 
                new Claim(ClaimTypes.Name, user.UserName), 
                new Claim(ClaimTypes.Email, user.Email) 
            };
        }


        internal ClaimsIdentityBuilder<TUser> AddRoles(IEnumerable<string> roles)
        {
            foreach (var role in roles)
                _claims.Add(new Claim(ClaimTypes.Role, role));

            return this;
        }

        internal ClaimsIdentityBuilder<TUser> AddClaims(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
                _claims.Add(claim);

            return this;
        }

        public Claim[] Build() => _claims.ToArray();

        
    }
}
