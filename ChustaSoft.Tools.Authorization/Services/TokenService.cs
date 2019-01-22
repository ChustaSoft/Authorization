﻿using ChustaSoft.Tools.Authorization.Configuration;
using ChustaSoft.Tools.Authorization.Constants;
using ChustaSoft.Tools.Authorization.Helpers;
using ChustaSoft.Tools.Authorization.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ChustaSoft.Tools.Authorization.Services
{
    public class TokenService : ITokenService
    {

        #region Fields

        private readonly IConfiguration _configuration;

        private readonly AuthorizationSettings _authorizationSettings;

        #endregion


        #region Constructor

        public TokenService(IConfiguration configuration, AuthorizationSettings authorizationSettings)
        {
            _configuration = configuration;

            _authorizationSettings = authorizationSettings;
        }

        #endregion


        #region Public methods

        public TokenInfo Generate(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = GenerateTokenDescriptor(user);
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenInfo(tokenHandler.WriteToken(token), token.ValidTo);
        }

        #endregion


        #region Private methods

        private SecurityTokenDescriptor GenerateTokenDescriptor(User user)
        {
            var claim = new[] { new Claim(JwtRegisteredClaimNames.Sub, user.UserName) };
            var signingKey = SecurityKeyHelper.GetSecurityKey(_configuration);
            var expiringDate = DateTime.UtcNow.AddMinutes(_authorizationSettings.MinutesToExpire);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            return new SecurityTokenDescriptor
            {
                Issuer = _authorizationSettings.SiteName,
                Audience = _authorizationSettings.SiteName,
                Subject = new ClaimsIdentity(claim),
                Expires = expiringDate,
                SigningCredentials = signingCredentials
            };
        }

        #endregion

    }
}
