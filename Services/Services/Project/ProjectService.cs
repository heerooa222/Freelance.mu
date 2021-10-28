using System;
using System.Collections.Generic;
using Services.Freelancer.DBProvider.Competence;
using Services.Freelancer.DBProvider.Project;
using Services.Freelancer.DBProvider.User;
using Services.Freelancer.Models;
using Services.Freelancer.Models.FrontOffice;
using Services.Freelancer.Services.User;

namespace Services.Freelancer.Services.Project
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectDataProvider _projectDataProvider;
        private readonly ICompetenceDataProvider _competenceDataProvider;
        private readonly IUserService _userService;
        private readonly IUserDataProvider _userDataProvider;

        public ProjectService(IProjectDataProvider projectDataProvider, ICompetenceDataProvider competenceDataProvider, IUserService userService, IUserDataProvider userDataProvider)
        {
            _projectDataProvider = projectDataProvider;
            _competenceDataProvider = competenceDataProvider;
            _userService = userService;
            _userDataProvider = userDataProvider;
        }

        public bool Apply(string projectId, string userId)
        {
            return _projectDataProvider.Apply(projectId, userId);
        }

        public int CountMyApplication(string userId)
        {
            return _projectDataProvider.CountMyApplication(userId);
        }

        public List<ProjectModel> GetMyProjects(string userIdUser, int affichage, int page)
        {
            return _projectDataProvider.GetMyProjects(userIdUser, affichage, page);
        }

        public int CountMyProjects(string userIdUser)
        {
            return _projectDataProvider.CountMyProjects(userIdUser);
        }

        public List<UserModel> GetApplicants(string projectId)
        {
            return _userDataProvider.GetApplicants(projectId);
        }

        public bool SelectUser(string projectId, string userId)
        {
            return _userDataProvider.SelectUser(projectId, userId);
        }

        public bool IsAlreadyApplied(string userId, string projetId)
        {
            return _projectDataProvider.IsAlreadyApplied(userId, projetId);
        }

        public List<ProjectModel> GetProjects(string? keyword, int affichage, int page)
        {
            return _projectDataProvider.GetProjects(keyword, affichage, page);
        }

        public List<ProjectModel> MyApplication(string userId, int affichage, int page)
        {
            return _projectDataProvider.GetMyApplication(userId, affichage, page);
        }

        public int CountProjects(string? keyword)
        {
            return _projectDataProvider.CountProjects(keyword);
        }

        public HomeModel GetHomeModel()
        {
            var homeModel = new HomeModel();
            try
            {
                homeModel.Projects = _projectDataProvider.GetLastOffers();
                homeModel.Competeces = _competenceDataProvider.GetAllCompetences();
                homeModel.NbCandidats = _userService.CountCandidat();
                homeModel.NbOffres = _userService.CountOffers();
                homeModel.NbEmployers = _userService.CountEmployer();
            }
            catch (Exception e)
            {
                //
            }

            return homeModel;
        }

        public List<CompetenceModel> GetAllCompetences()
        {
            return _competenceDataProvider.GetAllCompetences();
        }

        public ProjectModel GetProjetById(string projetId)
        {
            return _projectDataProvider.GetProjectById(projetId);
        }

        public bool PartagerOffre(ProjectModel model)
        {
            model.Description = $"<p class='p-t20'>{model.DescriptionEntreprise}</p>";
            model.Description += "<h5 class='font-weight-600'>Description</h5>";
            model.Description += "<div class='dez-divider divider-2px bg-gray-dark mb-4 mt-0'></div>";
            model.Description += $"<p>{model.DescriptionOffre}</p>";

            var splittedCompetences = model.SelectedCompetence.Split(',');
            model.CompetenceRequis = new List<CompetenceModel>();
            foreach (var competenceId in splittedCompetences)
            {
                if (!string.IsNullOrEmpty(competenceId))
                {
                    model.CompetenceRequis.Add(new CompetenceModel{IdCompetence = competenceId});
                }
            }
            
            return _projectDataProvider.SaveJob(model);
        }
    }
}