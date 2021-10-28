using System;
using System.Collections.Generic;

#nullable disable

namespace Services.Freelancer.Entities
{
    public partial class User
    {
        public User()
        {
            Applications = new HashSet<Application>();
            MessageReceiverNavigations = new HashSet<Message>();
            MessageSenderNavigations = new HashSet<Message>();
            Projects = new HashSet<Project>();
            UserCompetences = new HashSet<UserCompetence>();
        }

        public string IdUser { get; set; }
        public string TypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identifiant { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public DateTime? LastConnected { get; set; }
        public string CompanyName { get; set; }

        public virtual Type Type { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<Message> MessageReceiverNavigations { get; set; }
        public virtual ICollection<Message> MessageSenderNavigations { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<UserCompetence> UserCompetences { get; set; }
    }
}
