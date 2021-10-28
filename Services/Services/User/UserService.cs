using System;
using System.Collections.Generic;
using System.Linq;
using Services.Freelancer.DBProvider.User;
using Services.Freelancer.Models;
using Services.Freelancer.Utility;

namespace Services.Freelancer.Services.User
{
    public class UserService : IUserService
    {
        private IUserDataProvider _userDataProvider;

        public UserService(IUserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
        }

        public int CountCandidat()
        {
            return _userDataProvider.CountUser(UserType.FREELANCER);
        }

        public int CountEmployer()
        {
            return _userDataProvider.CountUser(UserType.EMPLOYER);
        }

        public int CountOffers()
        {
            return _userDataProvider.CountOffers();
        }

        public int CountResult(string nom, string type)
        {
            try
            {
                return _userDataProvider.CountResult(nom, type);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public List<UserModel> GetUserList(string type, string nom, int page, int affichage)
        {
            try
            {
                var users = _userDataProvider.GetUsers(type, nom, page, affichage);

                return users.Select(user => new UserModel()
                    {
                        Identifiant = user.Identifiant,
                        Mail = user.Mail,
                        Password = user.Password,
                        CompanyName = user.CompanyName,
                        FirstName = user.FirstName,
                        IdUser = user.IdUser,
                        LastName = user.LastName,
                        Competences = _userDataProvider.GetUserCompetences(user.IdUser).Select(x => new CompetenceModel() {CompetenceName = x.CompetenceName, IdCompetence = x.IdCompetence}).ToList()
                    })
                    .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public UserModel GetUser(UserModel model)
        {
            model.Password = Encryptor.HashPassword(model.Password);
            return _userDataProvider.GetUser(model);
        }

        public bool Register(RegisterModel model)
        {
            return _userDataProvider.Register(model);
        }
    }
}