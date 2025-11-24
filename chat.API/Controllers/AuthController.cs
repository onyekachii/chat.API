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
    [Authorize(AuthenticationSchemes = ApiKeyAuthHandler.SchemeName)]
    [ApiExplorerSettings(IgnoreApi = true)]
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
        public async Task<IActionResult> Register([FromHeader(Name = "app-id")] long appId,
            [FromBody] UserDTO model) // future reminder, if endpoint will take in data from frontend client, appid should be retreived some other way. AppId must never been known by client side (for security)  
        {
            var existingUser = await _service.UserService.GetUserAsync(model.Username, appId, false);
            if (existingUser != null) return Conflict("User already exists");

            var newUser = UserDTO.mapUserToDto( await _service.UserService.CreateUserAsync(model, appId), appId );
            await _service.db.SaveAsync();

            return CreatedAtAction(nameof(Register), new { username = newUser.Username });
        }

        [HttpPost("external")]
        public async Task<IActionResult> AuthenticateFromExternalService([FromHeader(Name = "app-id")] long appId,
            [FromBody] ExternalAuthRequestDTO model)
        {
            var user = await _service.UserService.GetUserAsync(model.username, appId, true);
            var tokens = await _service.AuthService.CreateTokensAsync(user!, (Role)model.role);
            await _service.db.SaveAsync();
            return Ok(new
            {
                accessToken = tokens.accessToken,
                refreshToken = RefreshTokenDTO.mapRefreshTokenToDto(tokens.refreshToken)
            });
        }

        [HttpPut("refresh")]
        public async Task<IActionResult> Refresh([FromHeader(Name = "app-id")] long appId, [FromBody] RefreshRequestDTO dto)
        {
            var stored = await _service.AuthService.GetRefreshTokenFromDbAsync(appId, dto.Username, dto.Token);
            if (stored == null || !stored.IsActive || stored.SoftDeleted) return Unauthorized();

            stored.IsUsed = true;
            stored.IsRevoked = true;

            var user = await _service.UserService.GetUserAsync(dto.Username, appId, true);
            var newTokens = await _service.AuthService.CreateTokensAsync(user!, (Role)dto.Role);

            stored.ReplacedByToken = newTokens.refreshToken.Token;
            _service.db.RefreshToken.Update(stored);

            await _service.db.SaveAsync();
            return Ok(new
            {
                accessToken = newTokens.accessToken,
                refreshToken = newTokens.refreshToken.Token,
                refreshTokenExpires = newTokens.refreshToken.Expires
            });
        }

        [HttpPut("revoke")]
        public async Task<IActionResult> Revoke([FromHeader(Name = "app-id")] long appId, [FromBody] RefreshTokenDTO dto)
        {
            var stored = await _service.AuthService.GetRefreshTokenFromDbAsync(appId, dto.Username, dto.Token);
            if (stored == null) return NotFound();

            stored.IsRevoked = true;
            _service.db.RefreshToken.Update(stored);
            await _service.db.SaveAsync();
            return NoContent();
        }
    }
}
