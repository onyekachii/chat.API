using chat.API.MiddleWare;
using chat.Domain.DTOs;
using chat.Service;
using chat.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IOptionsMonitor<JwtConfig> _jwtConfig;
        private readonly IServiceFactory _service;

        public AuthController(IOptionsMonitor<JwtConfig> jwtconfig, IServiceFactory service)
        {
            _jwtConfig = jwtconfig;
            _service = service;            
        }

        [HttpPost("register")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthHandler.SchemeName)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Register([FromHeader(Name = "app-id")] long appId,
            [FromBody] UserDTO model)
        {
            var existingUser = await _service.UserService.GetUserAsync(model.Username, appId);
            if (existingUser != null) return Conflict("User already exists");

            var newUser = UserDTO.mapUserToDto( await _service.UserService.CreateUserAsync(model, appId), appId );
            await _service.db.SaveAsync();

            return CreatedAtAction(nameof(Register), new { username = newUser.Username });
        }

        [HttpPost("external")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthHandler.SchemeName)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> AuthenticateFromExternalService([FromHeader(Name = "app-id")] long appId,
            [FromBody] ExternalAuthViewModel model)
        {
            var user = await _service.UserService.GetUserAsync(model.username, appId);
            var tokens = await _service.AuthService.CreateTokensAsync(user!);
            return Ok(new
            {
                accessToken = tokens.accessToken,
                refreshToken = RefreshTokenDTO.mapRefreshTokenToDto(tokens.refreshToken)
            });
        }

        [HttpPost("refresh")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthHandler.SchemeName)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Refresh([FromHeader(Name = "app-id")] long appId, [FromBody] RefreshTokenDTO dto)
        {
            var stored = await _service.AuthService.GetRefreshTokenFromDbAsync(appId, dto.Username, dto.Token);
            if (stored == null || !stored.IsActive || !stored.SoftDeleted) return Unauthorized();

            // Mark used (rotate)
            stored.IsUsed = true;
            stored.IsRevoked = true; // revoke old token; we'll store replacement

            var user = await _service.UserService.GetUserAsync(dto.Username, appId);

            var newTokens = await _service.AuthService.CreateTokensAsync(user!);

            // record replacement chain
            stored.ReplacedByToken = newTokens.refreshToken.Token;
            await _service.db.SaveAsync();

            return Ok(new
            {
                accessToken = newTokens.accessToken,
                refreshToken = newTokens.refreshToken.Token,
                refreshTokenExpires = newTokens.refreshToken.Expires
            });
        }

        [HttpPost("revoke")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthHandler.SchemeName)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Revoke([FromHeader(Name = "app-id")] long appId, [FromBody] RefreshTokenDTO dto)
        {
            var stored = await _service.AuthService.GetRefreshTokenFromDbAsync(appId, dto.Username, dto.Token);
            if (stored == null) return NotFound();

            stored.IsRevoked = true;
            await _service.db.SaveAsync();
            return NoContent();
        }
    }

    public record ExternalAuthViewModel(string username);
}
