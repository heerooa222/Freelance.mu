using System.Collections.Generic;
using Services.Freelancer.Models;
using Services.Freelancer.Models.FrontOffice;

namespace Services.Freelancer.Services.Project
{
    public interface IProjectService
    {
        HomeModel GetHomeModel();
        bool PartagerOffre(ProjectModel model);
        List<CompetenceModel> GetAllCompetences();
        ProjectModel GetProjetById(string projetId);
        List<ProjectModel> GetProjects(string? keyword, int affichage, int page);
        List<ProjectModel> MyApplication(string userId, int affichage, int page);
        int CountProjects(string? keyword);
        bool IsAlreadyApplied(string userId, string projetId);
        bool Apply(string projectId, string userId);
        int CountMyApplication(string userId);
        List<ProjectModel> GetMyProjects(string userIdUser, int affichage, int page);
        int CountMyProjects(string userIdUser);
        List<UserModel> GetApplicants(string projectId);
        bool SelectUser(string projectId, string userId);
    }
}