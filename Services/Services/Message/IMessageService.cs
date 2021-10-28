using System.Collections.Generic;
using Services.Freelancer.Models;

namespace Services.Freelancer.Services.Message
{
    public interface IMessageService
    {
        UserMessageModel LoadMessages(string id, string userId);
        List<MessageModel> GetMessages(string id, string userId);
        bool SendMessage(string id, string userId, string message);
    }
}