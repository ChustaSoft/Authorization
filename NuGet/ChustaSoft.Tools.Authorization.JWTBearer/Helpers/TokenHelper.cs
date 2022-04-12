using ChustaSoft.Tools.Authorization.Helpers;
using ChustaSoft.Tools.Authorization.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace ChustaSoft.Tools.Authorization
{
    public class TokenHelper<TUser> : ITokenHelper<TUser>
        where TUser : User, new()
    {

        #region Fields

        private readonly AuthorizationSettings _authorizationSettings;

        #endregion


        #region Constructor

        public TokenHelper(AuthorizationSettings authorizationSettings)
        {
            _authorizationSettings = authorizationSettings;
        }

        #endregion


        #region Public methods

        public TokenInfo Generate(TUser user, IEnumerable<string> roles, IEnumerable<Claim> claims, string privateKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var allClaims = GenerateClaims(user, roles, claims);
            var tokenDescriptor = GenerateTokenDescriptor(allClaims, privateKey);
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenInfo(tokenHandler.WriteToken(token), token.ValidTo);
        }

        #endregion


        #region Private methods

        private SecurityTokenDescriptor GenerateTokenDescriptor(Claim[] claims, string privateKey)
        {
            var signingKey = SecurityKeyHelper.GetSecurityKey(privateKey);
            var expiringDate = DateTime.UtcNow.AddMinutes(_authorizationSettings.MinutesToExpire);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            return new SecurityTokenDescriptor
            {
                Issuer = _authorizationSettings.SiteName,
                Audience = _authorizationSettings.SiteName,
                Subject = new ClaimsIdentity(claims),
                Expires = expiringDate,
                SigningCredentials = signingCredentials
            };
        }

        private Claim[] GenerateClaims(TUser user, IEnumerable<string> roles, IEnumerable<Claim> claims)
        {
            var claimsBuilder = new ClaimsIdentityBuilder<TUser>(user)
                .AddRoles(roles)
                .AddClaims(claims);

            return claimsBuilder.Build();
        }

        #endregion

    }



    public class TokenHelper : TokenHelper<User>, ITokenHelper
    {

        public TokenHelper(AuthorizationSettings authorizationSettings)
            : base(authorizationSettings)
        { }

    }

}
