using chat.Domain.DTOs;
using chat.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using static chat.Domain.DTOs.GroupTypes;

namespace chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : BaseController
    {
        private readonly IServiceFactory _service;

        public GroupController(IServiceFactory service)
        {
            _service = service;           
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup(GroupPostRequestDTO dto)
        {            
            var result = await _service.GroupService.CreateGroupAsync(dto, UserInfo!);            
            await _service.db.SaveAsync();
            return CreatedAtAction(nameof(CreateGroup), new { result = GroupResponseDTO.mapGroupToDto(result) });
        }
    }
}