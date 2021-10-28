using System.Collections.Generic;
using System.Threading.Tasks;

namespace Freelancer.mu.Classes.Notification.Sender
{
    public interface INotificationSender
    {
        Task ReloadMessage(string receiverId, string myId);
    }

}