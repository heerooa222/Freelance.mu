using System;
using System.Collections.Generic;
using System.Linq;
using Services.Freelancer.Entities;
using Services.Freelancer.Models;

namespace Services.Freelancer.DBProvider.Competence
{
    public class CompetenceDataProvider : ICompetenceDataProvider
    {
        private DatabaseContext _context;

        public CompetenceDataProvider(DatabaseContext context)
        {
            _context = context;
        }

        public List<CompetenceModel> GetAllCompetences()
        {
            try
            {
                var competences = _context.Competence.ToList();
                return competences.Select(competence => new CompetenceModel() {CompetenceName = competence.CompetenceName, IdCompetence = competence.IdCompetence}).ToList();
            }
            catch (Exception e)
            {
                return new List<CompetenceModel>();
            }
        }
    }
}