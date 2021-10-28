using System;

#nullable disable

namespace Services.Freelancer.Entities
{
    public partial class Message
    {
        public string IdMessage { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Content { get; set; }
        public DateTime MessageDate { get; set; }
        public bool IsSeen { get; set; }
        public DateTime? DateSeen { get; set; }

        public virtual User ReceiverNavigation { get; set; }
        public virtual User SenderNavigation { get; set; }
    }
}
