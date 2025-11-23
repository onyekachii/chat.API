using chat.Domain.Entities;
using chat.Repo;
using chat.Service.Interface;
using chat.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace chat.Service.Implementation
{
    public class AuthService : IAuthService
    {
        private IRepoFactory _db;
        private readonly IOptionsMonitor<JwtConfig> _jwtSettings;

        public AuthService(IRepoFactory db, IOptionsMonitor<JwtConfig> jwtSettings)
        {
            _db = db;
            _jwtSettings = jwtSettings;
        }
        public async Task<string> GetApiKeyAsync(long appId)
        {
            var key = await _db.ApiKey.FindByCondition(a => a.AppID == appId 
            && a.SoftDeleted == false && !a.Revoked).SingleOrDefaultAsync();
            if (key == null) 
                throw new Exception( "Api key not found for the given App ID." );
            return key.Key;
        }
        public async Task<bool> IsApiKeyValid(string apiKeyFromCLient, string apiKey)
            => string.Equals(apiKeyFromCLient, apiKey);

        public async Task<(string accessToken, RefreshToken refreshToken)> CreateTokensAsync(User userModel)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.CurrentValue.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var user = _db.User.FindByCondition(u => u.Username == userModel.Username && u.AppId == userModel.AppId).SingleOrDefault();
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _jwtSettings.CurrentValue.Issuer,
                audience: _jwtSettings.CurrentValue.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.CurrentValue.AccessTokenExpirationMinutes),
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshTokenString(),
                UserName = user.Username,
                Expires = DateTimeOffset.UtcNow.AddDays(_jwtSettings.CurrentValue.RefreshTokenExpirationDays),
                CreatedDate = DateTimeOffset.UtcNow,
            };
            await _db.RefreshToken.CreateAsync(refreshToken);
            return (accessToken, refreshToken);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _jwtSettings.CurrentValue.Audience,
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.CurrentValue.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.CurrentValue.Key)),
                ValidateLifetime = false // we want expired tokens here
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                if (securityToken is not JwtSecurityToken jwt || !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<RefreshToken?> GetRefreshTokenFromDbAsync(long appId, string username, string token)
        {
            return await _db.RefreshToken.FindByCondition(r => string.Equals(r.Token, token )
            && r.AppID == appId && string.Equals(r.UserName, username)).SingleOrDefaultAsync();

        }

    }
}
