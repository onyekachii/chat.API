using chat.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace chat.API.MiddleWare
{
    public class ApiKeyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "ApiKey";
        private readonly IAuthService _authservice;

        public ApiKeyAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, IAuthService authService)
            : base(options, logger, encoder)
        {
            _authservice = authService;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("chat-api-key", out var apiKeyFromCLient))
                return Task.FromResult(AuthenticateResult.Fail("Missing API key"));

            if (!Request.Headers.TryGetValue("app-id", out var appId))
                return Task.FromResult(AuthenticateResult.Fail("Missing API key"));
            
            long.TryParse(appId, out var appID);

            var validKey = _authservice.GetApiKey(appID).Result;
            if (!_authservice.IsApiKeyValid(apiKeyFromCLient!, validKey).Result)
                return Task.FromResult(AuthenticateResult.Fail("Invalid API key"));

            var identity = new ClaimsIdentity(SchemeName);
            var principal = new ClaimsPrincipal(identity);

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, SchemeName))); ;
        }
    }
}
