using chat.Domain.Entities;
using chat.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace chat.Service.Interface
{
    public interface IAuthService
    {
        Task<string> GetApiKeyAsync(long appId);
        Task<bool> IsApiKeyValid(string apiKeyFromCLient, string apiKey);
        Task<(string accessToken, RefreshToken refreshToken)> CreateTokensAsync(User user, Role r);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        Task<RefreshToken> GetRefreshTokenFromDbAsync(long appId, string username, string token);
    }
}
