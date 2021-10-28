using System;
using System.Threading.Tasks;
using Freelancer.mu.Classes.Notification.Connection;
using Microsoft.AspNetCore.SignalR;

namespace Freelancer.mu.Classes.Notification
{
    public class NotificationHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;

        public NotificationHub(IUserConnectionManager userConnectionManager)
        {
            _userConnectionManager = userConnectionManager;
        }
        
        public string GetConnectionId()
        {
            var httpContext = this.Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];
            _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);

            return Context.ConnectionId;
        }
        
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;
            _userConnectionManager.RemoveUserConnection(connectionId);
            var value = await Task.FromResult(0);
        }
    }
}