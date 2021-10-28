using System.Collections.Generic;

#nullable disable

namespace Services.Freelancer.Entities
{
    public partial class Competence
    {
        public Competence()
        {
            ProjectCompetences = new HashSet<ProjectCompetence>();
            UserCompetences = new HashSet<UserCompetence>();
        }

        public string IdCompetence { get; set; }
        public string CompetenceName { get; set; }

        public virtual ICollection<ProjectCompetence> ProjectCompetences { get; set; }
        public virtual ICollection<UserCompetence> UserCompetences { get; set; }
    }
}
