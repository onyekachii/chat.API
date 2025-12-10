using chat.Domain.DTOs;
using chat.Domain.Entities;
using chat.Repo;
using chat.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace chat.API
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _uow;
        private readonly IMessageService _messageService;
        private readonly IGroupService _groupService;

        public ChatHub(IUnitOfWork uow, IGroupService groupService, IMessageService messageService)
        {
            _uow = uow;
            _groupService = groupService;
            _messageService = messageService;
        }

        public async Task SendMessageToGroup(string groupName, string displayName, string message, string methodIdentifier)
        {
            //var userId = Context.UserIdentifier ?? Context.ConnectionId;
            //var userName = Context.User?.Identity?.Name ?? "anon";
            long.TryParse(Context.User?.FindFirst("AppId")?.Value, out var appId);
            var group = await _groupService.GetGroupByNameAsync(groupName, appId, true);
            var username = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException();

            var m = new Message
            {
                GroupId = group!.ID,
                Text = message,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow,
                DisplayName = displayName
            };

            Clients.Group($"Message-{methodIdentifier}").SendAsync($"Message-{methodIdentifier}", new
            {
                GroupName = groupName,
                CreatedBy = username,
                Text = message,
                DisplayName = displayName,
                CreatedDate = m.CreatedDate
            });

            await _uow.Message.CreateAsync(m);
            await _uow.SaveAsync();            
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task JoinGroup(string groupName) =>
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        public async Task LeaveGroup(string groupName) =>
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

    }
}
