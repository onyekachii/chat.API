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
        private IUnitOfWork _db;
        private readonly IOptionsMonitor<JwtConfig> _jwtSettings;
        private readonly IUserService _userService;
        readonly IAppService _appService;
    

        public AuthService(IUnitOfWork db, IOptionsMonitor<JwtConfig> jwtSettings,
            IUserService userService, IAppService appService)
        {
            _db = db;
            _jwtSettings = jwtSettings;
            _userService = userService;
            _appService = appService;
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

        public async Task<(string accessToken, RefreshToken refreshToken)> CreateTokensAsync(User userModel, Role role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.CurrentValue.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var user = await _userService.GetUserAsync(userModel.Username, userModel.AppId, true);
            var app = await _appService.GetAppAsync(userModel.AppId, true);
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user!.Username),
                    new Claim("Role", role.ToString()),
                    new Claim("AppId", user.AppId.ToString())
                };

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _jwtSettings.CurrentValue.Issuer,
                audience: _jwtSettings.CurrentValue.Audience,
                claims: claims,
                expires: app!.JwtAccessExpiryMinutes > 0 ? DateTime.UtcNow.AddMinutes(app.JwtAccessExpiryMinutes) : DateTime.UtcNow.AddMinutes(_jwtSettings.CurrentValue.AccessTokenExpirationMinutes),
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshTokenString(),
                UserName = user.Username,
                AppID = userModel.AppId,
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.CurrentValue.RefreshTokenExpirationDays),
                CreatedDate = DateTime.UtcNow,
            };
            return (accessToken, (await _db.RefreshToken.CreateAsync(refreshToken)).Entity);
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
