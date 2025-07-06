using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyDotNetApp.Models;

namespace MyDotNetApp.Helpers
{
    public class JwtHelper
    {
        public static string GenerateJwtToken(User user)
        {
            // Replace with your hardcoded settings
            var key = "YourSuperSecretKeyForJWTAuthentication123456";
            var issuer = "http://localhost:3000";
            var audience = "http://localhost:3000";
            var tokenValidityMins = 30;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "JwtSubject"),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenValidityMins),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
