using chat.Repo;
using chat.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static chat.Domain.DTOs.GroupTypes;

namespace chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;
        private readonly IGroupUserService _groupUserService;
        public GroupController(IUnitOfWork uow, IGroupService groupService,
            IUserService userService, IGroupUserService groupUserService)
        {
            _uow = uow;
            _groupService = groupService;
            _userService = userService;
            _groupUserService = groupUserService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup(GroupPostRequestDTO dto)
        {
            var result = await _groupService.CreateGroupAsync(dto, UserInfo!);            
            await _uow.SaveAsync();
            return CreatedAtAction(nameof(CreateGroup), new { result = GroupResponseDTO.mapGroupToDto(result) });
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetGroups(int pageSize, int page, DateTime? lastCreated = null)
        {
            var res = await _groupService.GetAllGroups(lastCreated, pageSize, page, UserInfo!.AppID);
            if (res is not null)
            {
                var result = await Task.WhenAll(res.Select(r => Task.Run(() => GroupResponseDTO.mapGroupToDto(r))));
                return Ok(result);
            }
            return Ok();
        }
       
        [HttpPost("join")]
        public async Task<IActionResult> JoinGroup(JoinGroupRequestDTO dto)
        {
            var group = await _groupService.GetGroupAsync(dto.GroupID, UserInfo!.AppID, true);
            var user = await _userService.GetUserAsync(dto.UserName, UserInfo!.AppID, true);
            // check if user is already in group

            await _groupUserService.CreateGroupUserAsync(group!, user!);
            await _uow.SaveAsync();
            return Ok();
            
        }
    }
}