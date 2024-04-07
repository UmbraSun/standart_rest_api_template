using DAL.Models;
using DTOs;
using DTOs.Models;
using Microsoft.AspNetCore.Identity;
using Resources.Words;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using template_asp.net_application.Infrastructure;
using Microsoft.Extensions.Options;

namespace template_asp.net_application.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly Auth.Jwt _jwt;

        public AuthService(UserManager<User> userManager, IOptions<Auth.Jwt> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }

        public async Task<Auth.Responce> AccessToken(Auth.Login dto)
        {
            var (user, claims, roleNames) = await UserClaimsRoles(dto.Username, dto.Password);
            return BuildResponce(user, claims, roleNames);
        }

        private async Task<(User user, IEnumerable<Claim> claims, IEnumerable<string> roles)> UserClaimsRoles(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                throw new Exception(Resource.UserNotFound);
            if (!await _userManager.CheckPasswordAsync(user, password))
                throw new Exception(Resource.IncorrectPassword);

            var (claims, roles) = await ClaimsAndRoles(user);
            return (user, claims, roles);
        }

        public async Task<(IEnumerable<Claim> claims, IEnumerable<string> roles)> ClaimsAndRoles(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new(ClaimTypes.NameIdentifier, user.Id)
            };
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            return (claims, roles);
        }

        private Auth.Responce BuildResponce(User user, IEnumerable<Claim> claims, IEnumerable<string> roles)
        {
            var (accessToken, accessExpireDate) = AccessToken(claims);
            var now = DateTime.UtcNow;
            var refreshExpireDate = now + TimeSpan.FromMinutes(_jwt.RefreshTokenLifeTimeInMinutes);
            var refreshToken = new RefreshToken(user.UserName, now, refreshExpireDate);
            var jsonToken = JsonSerializer.Serialize(refreshToken);
            var encriptToken = StringCipher.Encrypt(jsonToken);

            return new Auth.Responce
            {
                AccessToken = accessToken,
                AccessTokenExpiteDate = accessExpireDate,
                RefresgToken = encriptToken,
                RefreshTokenExpireTime = refreshExpireDate,
                Roles = roles,
                UserName = user.UserName
            };
        }

        private (string AccessToken, DateTime Expires) AccessToken(IEnumerable<Claim> claims)
        {
            var (token, expires) = JwtBuilder.SecurityToken(claims, _jwt);
            return new(new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        public async Task<Auth.Responce> RefreshToken(Auth.Refresh refresh)
        {
            var buffer = new byte[refresh.RefreshToken.Length];
            if (Convert.TryFromBase64String(refresh.RefreshToken, buffer, out int bytesParsed))
                throw new Exception(Resource.TheTokenMustNotBeInBase64Format);

            var decript = StringCipher.Decrypt(refresh.RefreshToken);
            var refreshToken = JsonSerializer.Deserialize<RefreshToken>(decript);
            if (refreshToken is null || refreshToken.ExpireDate <= DateTime.UtcNow)
                throw new Exception(Resource.TheRefreshTokenHasExpired);

            var user = await _userManager.FindByNameAsync(refreshToken.UserName);
            var (claims, roles) = await ClaimsAndRoles(user);
            return BuildResponce(user, claims, roles);
        }
    }
}
