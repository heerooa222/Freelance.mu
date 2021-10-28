using System;

#nullable disable

namespace Services.Freelancer.Entities
{
    public partial class Application
    {
        public string IdApplication { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int IsAssignedTo { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}
