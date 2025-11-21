using chat.Domain.Entities;
using System.Security.Claims;

namespace chat.Service.Interface
{
    public interface ITokenService
    {
        Task<(string accessToken, RefreshToken refreshToken)> CreateTokensAsync(User user, string ipAddress);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}