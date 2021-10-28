using System.Collections.Generic;

namespace Freelancer.mu.Classes.Notification.Connection
{
    public interface IUserConnectionManager
    {
        void KeepUserConnection(string userId, string connectionId);
        void RemoveUserConnection(string connectionId);
        List<string> GetUserConnections(string userId);
        List<string> GetAllConnection();
    }
}