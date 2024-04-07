using DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace template_asp.net_application.Infrastructure
{
    internal static class JwtBuilder
    {
        internal static TokenValidationParameters Parameters(IConfiguration configuration)
        {
            var settings = configuration.GetSection(nameof(Auth.Jwt)).Get<Auth.Jwt>();
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = settings.Issuer,
                ValidAudience = settings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
                ClockSkew = TimeSpan.Zero
            };
        }

        internal static (JwtSecurityToken Token, DateTime Expire) SecurityToken(IEnumerable<Claim> claims, Auth.Jwt jwt)
        {
            var utc = DateTime.UtcNow;
            var expires = utc.Add(TimeSpan.FromMinutes(jwt.AccessTokenLifeTimeInMinutes));
            return (new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
                    utc,
                    expires,
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt.Key)), SecurityAlgorithms.HmacSha256Signature)), 
                expires);
        }

        internal static string Bearer()
        {
            return nameof(Bearer).ToLower();
        }

        internal static string Authorization()
        {
            return nameof(Authorization);
        }
    }
}
