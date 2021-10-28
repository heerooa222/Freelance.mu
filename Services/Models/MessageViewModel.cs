using System.Collections.Generic;

namespace Services.Freelancer.Models
{
    public class MessageViewModel
    {
        public UserMessageModel? LastMessageModel { get; set; }
        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
    }
}