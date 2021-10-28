#nullable disable

namespace Services.Freelancer.Entities
{
    public partial class ProjectCompetence
    {
        public string CompetenceId { get; set; }
        public string ProjectId { get; set; }

        public virtual Competence Competence { get; set; }
        public virtual Project Project { get; set; }
    }
}
