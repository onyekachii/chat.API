using chat.API.MiddleWare;
using chat.Domain.DTOs;
using chat.Repo;
using chat.Service;
using chat.Service.Interface;
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
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _uow;

        public AuthController(IOptionsMonitor<JwtConfig> jwtconfig, 
            IUserService userService, IUnitOfWork uow, IAuthService authService)
        {
            _jwtConfig = jwtconfig;
            _userService = userService;
            _uow = uow;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromHeader(Name = "app-id")] long appId,
            [FromBody] UserDTO model) 
        {
            var existingUser = await _userService.GetUserAsync(model.Username, appId, false);
            if (existingUser != null) 
                return Conflict("User already exists");

            var newUser = UserDTO.mapUserToDto( await _userService.CreateUserAsync(model, appId), appId );
            await _uow.SaveAsync();

            return CreatedAtAction(nameof(Register), new { username = newUser.Username });
        }

        [HttpPost("external")]
        public async Task<IActionResult> AuthenticateFromExternalService([FromHeader(Name = "app-id")] long appId,
            [FromBody] ExternalAuthRequestDTO model)
        {
            var user = await _userService.GetUserAsync(model.username, appId, true);
            var tokens = await _authService.CreateTokensAsync(user!, (Role)model.role);
            await _uow.SaveAsync();
            return Ok(new 
            {
                AccessToken = tokens.accessToken,
                RefreshToken = RefreshTokenDTO.mapRefreshTokenToDto(tokens.refreshToken)
            });
        }

        [HttpPut("refresh")]
        public async Task<IActionResult> Refresh([FromHeader(Name = "app-id")] long appId, [FromBody] RefreshRequestDTO dto)
        {
            var stored = await _authService.GetRefreshTokenFromDbAsync(appId, dto.Username, dto.Token);
            if (stored == null || !stored.IsActive || stored.SoftDeleted) return Unauthorized();

            stored.IsUsed = true;
            stored.IsRevoked = true;

            var user = await _userService.GetUserAsync(dto.Username, appId, true);
            var newTokens = await _authService.CreateTokensAsync(user!, (Role)dto.Role);

            stored.ReplacedByToken = newTokens.refreshToken.Token;
            _uow.RefreshToken.Update(stored);

            await _uow.SaveAsync();
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
            var stored = await _authService.GetRefreshTokenFromDbAsync(appId, dto.Username, dto.Token);
            if (stored == null) return NotFound();

            stored.IsRevoked = true;
            _uow.RefreshToken.Update(stored);
            await _uow.SaveAsync();
            return NoContent();
        }
    }
}
