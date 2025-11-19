using chat.Domain.DTOs;
using chat.Domain.Entities;
using chat.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace chat.API
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IRepoFactory _factory;
         
        public ChatHub(IRepoFactory factory)
        {
            _factory = factory;
        }

        public async Task SendMessageToRoom(long groupId, string message)
        {
            var userId = Context.UserIdentifier ?? Context.ConnectionId;
            var userName = Context.User?.Identity?.Name ?? "anon";

            var m = new Message
            {
                GroupId = groupId > 0 ? groupId : null,
                Text = message,
                //CreatedBy = userId
            };

            Clients.Group(groupId.ToString()).SendAsync("ReceiveMessage", new
            {
                GroupId = m.GroupId,
                Text = m.Text,
            });

            
            await _factory.Message.CreateAsync(m);
            await _factory.SaveAsync();            
        }

        public override async Task OnConnectedAsync()
        {
            // Optionally set UserIdentifier (if using sub claim, configure mapping)
            await base.OnConnectedAsync();
        }

        public async Task JoinRoom(string roomName) =>
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

        public async Task LeaveRoom(string roomName) =>
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);

    }
}
