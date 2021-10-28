#nullable disable

namespace Services.Freelancer.Entities
{
    public partial class UserCompetence
    {
        public string UserId { get; set; }
        public string CompetenceId { get; set; }

        public virtual Competence Competence { get; set; }
        public virtual User User { get; set; }
    }
}
