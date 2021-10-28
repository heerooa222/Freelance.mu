using System.Collections.Generic;
using Services.Freelancer.Models;

namespace Services.Freelancer.DBProvider.Project
{
    public interface IProjectDataProvider
    {
        List<ProjectModel> GetLastOffers();
        bool SaveJob(ProjectModel model);
        ProjectModel GetProjectById(string projetId);
        List<ProjectModel> GetProjects(string? keyword, int affichage, int page);
        int CountProjects(string? keyword);
        bool IsAlreadyApplied(string userId, string projetId);
        bool Apply(string projectId, string userId);
        List<ProjectModel> GetMyApplication(string userId, int affichage, int page);
        int CountMyApplication(string userId);
        int CountMyProjects(string userIdUser);
        List<ProjectModel> GetMyProjects(string userIdUser, int affichage, int page);
    }
}