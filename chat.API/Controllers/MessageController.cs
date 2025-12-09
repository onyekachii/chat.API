using chat.Domain.DTOs;
using chat.Repo;
using chat.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static chat.Domain.DTOs.GroupTypes;

namespace chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : BaseController
    {

        private readonly IUnitOfWork _uow;
        private readonly IMessageService _messageService;

        public MessageController(IUnitOfWork uow, IMessageService messageService)
        {
            _uow = uow;
            _messageService = messageService;
        }

        [HttpGet("group")]
        public async Task<IActionResult> GetMessagesByGroup(string groupName, DateTime? lastCreated = null)
        {
            var res = await _messageService.GetMessagesByName(lastCreated, groupName, UserInfo!.AppID);
            if (res.Count > 0)
            {
                var result = await Task.WhenAll(res.Select(r => Task.Run(() => MessageResponseDTO.mapMessageToDto(r))));
                return Ok(result);
            }
            return Ok();
        }
    }
}
