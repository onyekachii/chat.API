using chat.Domain.Entities;
using chat.Repo;
using chat.Service.Interface;
using chat.Service.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace chat.Service.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IOptionsMonitor<JwtConfig> _jwtSettings;
        private IRepoFactory _repoFactory;
        public TokenService(IOptionsMonitor<JwtConfig> jwtSettings, IRepoFactory repoFactory)
        {
            _jwtSettings = jwtSettings;
            _repoFactory = repoFactory;
        }

        public async Task<(string accessToken, RefreshToken refreshToken)> CreateTokensAsync(User userModel, string ipAddress)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.CurrentValue.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var user = _repoFactory.User.FindByCondition(u => u.Username == userModel.Username && u.AppId == userModel.AppId).SingleOrDefault();
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
                UserId = user.Username,
                Expires = DateTimeOffset.UtcNow.AddDays(_jwtSettings.CurrentValue.RefreshTokenExpirationDays),
                CreatedDate = DateTimeOffset.UtcNow,
                CreatedByIp = ipAddress // is IP address still a good variable since it could change before expiry
            };
            await _repoFactory.RefreshToken.CreateAsync(refreshToken);
            await _repoFactory.SaveAsync();
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
    }
}
