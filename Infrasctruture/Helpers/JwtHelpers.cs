using Microsoft.IdentityModel.Tokens;
using Munizoft.Identity.Entities;
using Munizoft.Identity.Infrastructure.Models;
using Munizoft.Identity.Resources.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Munizoft.Identity.Infrastructure.Helpers
{
    public static class JwtHelpers
    {
        public static async Task<LoginResponseResource> GenerateToken(JwtOptions options, User user, List<String> roles)
        {
            var claims = new List<Claim>
            {
                new Claim("user_id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roles.Any() ? roles.FirstOrDefault() : String.Empty)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(options.ExpireDays);

            var token = new JwtSecurityToken(
                options.Issuer,
                options.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new LoginResponseResource()
            {
                UserId = user.Id,
                Username = user.Email,
                AccessToken = accessToken,
                ExpiresIn = expires
            };

            return response;
        }
    }
}
