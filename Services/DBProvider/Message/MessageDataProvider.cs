using System;
using System.Collections.Generic;
using System.Linq;
using Services.Freelancer.Entities;
using Services.Freelancer.Models;

namespace Services.Freelancer.DBProvider.Message
{
    public class MessageDataProvider : IMessageDataProvider
    {
        private readonly DatabaseContext _context;

        public MessageDataProvider(DatabaseContext context)
        {
            _context = context;
        }

        public UserMessageModel LoadMessages(string id, string userId)
        {
            try
            {
                var user = _context.User.FirstOrDefault(x => x.IdUser == id);
                if (user != null)
                {
                    var result = new UserMessageModel
                    {
                        UserId = id,
                        UserName = $"{user.FirstName} {user.LastName}"
                    };

                    try
                    {
                        var lastMessage = _context.Message
                            .Where(x => (x.Receiver == userId && x.Sender == id) ||
                                        (x.Sender == userId && x.Receiver == id)).OrderByDescending(x => x.MessageDate).Take(1).ToList();
                        foreach (var message in lastMessage)
                        {
                            result.IsRead = message.IsSeen;
                            result.LastMessage = message.Content;
                            result.LastMessageDate = message.MessageDate;
                        }
                    }
                    catch
                    {
                        //ignored
                    }

                    return result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<MessageModel> GetMessages(string id, string userId)
        {
            try
            {
                var messages = _context.Message.Where(x => (x.Receiver == userId && x.Sender == id) ||
                                                           (x.Sender == userId && x.Receiver == id))
                    .OrderBy(x => x.MessageDate).ToList();

                return messages.Select(message => new MessageModel { Message = message.Content, IsMe = message.Sender == userId, MessageDate = message.MessageDate }).ToList();
            }
            catch
            {
                return new();
            }
        }

        public bool SendMessage(string id, string userId, string message)
        {
            try
            {
                var messageData = new Entities.Message
                {
                    Content = message,
                    Receiver = id,
                    Sender = userId,
                    IdMessage = Guid.NewGuid().ToString(),
                    IsSeen = false,
                    MessageDate = DateTime.Now
                };
                _context.Message.Add(messageData);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}