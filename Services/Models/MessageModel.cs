using System;

namespace Services.Freelancer.Models
{
    public class MessageModel
    {
        public DateTime MessageDate { get; set; }
        public string Message { get; set; }
        public bool IsMe { get; set; }
        public string DisplayMessage => DisplayDate();

        private string DisplayDate()
        {
            return $"{MessageDate:hh:mm} | {MessageDate:MMM dd}";
        }
    }
}