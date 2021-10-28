using System.Collections.Generic;
using Services.Freelancer.Models;

namespace Services.Freelancer.Services.User
{
    public interface IUserService
    {
        int CountCandidat();
        int CountEmployer();
        int CountOffers();
        int CountResult(string nom, string type);
        List<UserModel> GetUserList(string type, string nom, int page, int affichage);

        UserModel GetUser(UserModel model);
        bool Register(RegisterModel model);
    }
}