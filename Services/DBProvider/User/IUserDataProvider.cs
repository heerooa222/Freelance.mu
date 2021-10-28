using System.Collections.Generic;
using Services.Freelancer.Models;

namespace Services.Freelancer.DBProvider.User
{
    public interface IUserDataProvider
    {
        int CountUser(string type);
        int CountOffers();
        List<Entities.User> GetUsers(string type, string nom, int page, int affichage);
        List<Entities.Competence> GetUserCompetences(string userId);
        int CountResult(string nom, string type);
        UserModel GetUser(UserModel model);
        bool SelectUser(string projectId, string userId);
        List<UserModel> GetApplicants(string projectId);
        bool Register(RegisterModel model);
    }
}