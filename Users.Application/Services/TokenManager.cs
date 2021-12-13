using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Users.Application.Services
{
    public static class TokenManager
    {
        public static string GenerateToken(string username)
        {
            byte[] key = Convert.FromBase64String(ConfigurationManager.AppSettings["JWT_Secret"]);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[] { new Claim(ClaimTypes.Name, username) }
                ),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            return handler.WriteToken(token);
        }

        public static string ValidateToken(string token)
        {
            ClaimsPrincipal principal = GetPrincipal(token);

            if (principal == null)
                return null;

            ClaimsIdentity identity;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            return identity.FindFirst(ClaimTypes.Name)?.Value ?? null;
        }

        private static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                    return null;

                byte[] key = Convert.FromBase64String(ConfigurationManager.AppSettings["JWT_Secret"]);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out SecurityToken securityToken);

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
