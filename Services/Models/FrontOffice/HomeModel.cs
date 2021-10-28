using System.Collections.Generic;

namespace Services.Freelancer.Models.FrontOffice
{
    public class HomeModel
    {
        public List<ProjectModel> Projects { get; set; }
        public List<CompetenceModel> Competeces { get; set; }
        public int NbOffres { get; set; }
        public int NbEmployers { get; set; }
        public int NbCandidats { get; set; }
    }
}