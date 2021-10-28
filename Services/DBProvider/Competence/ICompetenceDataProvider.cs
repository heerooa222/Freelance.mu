using System.Collections.Generic;
using Services.Freelancer.Models;

namespace Services.Freelancer.DBProvider.Competence
{
    public interface ICompetenceDataProvider
    {
        List<CompetenceModel> GetAllCompetences();
    }
}