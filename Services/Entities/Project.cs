using System;
using System.Collections.Generic;

#nullable disable

namespace Services.Freelancer.Entities
{
    public partial class Project
    {
        public Project()
        {
            Applications = new HashSet<Application>();
            ProjectCompetences = new HashSet<ProjectCompetence>();
        }

        public string IdProject { get; set; }
        public string AddedBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime ApplyBefore { get; set; }
        public decimal? Price { get; set; }

        public virtual User AddedByNavigation { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<ProjectCompetence> ProjectCompetences { get; set; }
    }
}
