using HealthApp.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using HealthApp.Application.Common;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace HealthApp.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtToken;

        public TokenService(IOptions<JwtOptions> jwtOption)
        {
            _jwtToken = jwtOption.Value;
        }

        public string GenerateToken(int UserId, string userName)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtToken.SecretKey));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer : _jwtToken.Issuer,
                audience : _jwtToken.Audience,
                claims : claims,
                expires : DateTime.UtcNow.AddMinutes(_jwtToken.ExpiryMinutes),
                signingCredentials : cred
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
