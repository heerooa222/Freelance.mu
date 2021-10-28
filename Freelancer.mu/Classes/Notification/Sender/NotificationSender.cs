using System.Collections.Generic;
using System.Threading.Tasks;
using Freelancer.mu.Classes.Notification.Connection;
using Microsoft.AspNetCore.SignalR;

namespace Freelancer.mu.Classes.Notification.Sender
{
    public class NotificationSender : INotificationSender
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IUserConnectionManager _userConnectionManager;

        public NotificationSender(IHubContext<NotificationHub> notificationHubContext, IUserConnectionManager userConnectionManager)
        {
            _notificationHubContext = notificationHubContext;
            _userConnectionManager = userConnectionManager;
        }

        public async Task ReloadMessage(string receiverId, string senderId)
        {
            try
            {
                var connections = _userConnectionManager.GetUserConnections(receiverId);
                foreach (var connection in connections)
                {  
                    await _notificationHubContext.Clients.Client(connection).SendAsync("ReloadMessage", receiverId, senderId);
                }
            }
            catch
            {
                //ignored
            }
        }
    }
}