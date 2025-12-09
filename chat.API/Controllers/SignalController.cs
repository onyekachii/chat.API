using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SignalController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hub;
        public SignalController(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }


    }
}
