using System.Collections.Generic;
using Services.Freelancer.DBProvider.Message;
using Services.Freelancer.Models;

namespace Services.Freelancer.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IMessageDataProvider _messageDataProvider;

        public MessageService(IMessageDataProvider messageDataProvider)
        {
            _messageDataProvider = messageDataProvider;
        }

        public UserMessageModel LoadMessages(string id, string userId)
        {
            return _messageDataProvider.LoadMessages(id, userId);
        }

        public List<MessageModel> GetMessages(string id, string userId)
        {
            return _messageDataProvider.GetMessages(id, userId);
        }

        public bool SendMessage(string id, string userId, string message)
        {
            return _messageDataProvider.SendMessage(id, userId, message);
        }
    }
}