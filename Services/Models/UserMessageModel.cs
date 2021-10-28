using System;

namespace Services.Freelancer.Models
{
    public class UserMessageModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? LastMessageDate { get; set; }
        public string DisplayLastMessageDate => GetDate();
        public string LastMessage { get; set; }
        public bool IsRead { get; set; }

        private string GetDate()
        {
            if (LastMessageDate != null)
            {
                return $"{LastMessageDate:dd MMM}";
            }

            return "";
        }
    }
}