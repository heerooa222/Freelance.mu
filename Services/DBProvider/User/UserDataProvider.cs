using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Freelancer.Entities;
using Services.Freelancer.Models;
using Services.Freelancer.Utility;

namespace Services.Freelancer.DBProvider.User
{
    public class UserDataProvider : IUserDataProvider
    {
        private readonly DatabaseContext _context;

        public UserDataProvider(DatabaseContext context)
        {
            _context = context;
        }

        public UserModel GetUser(UserModel model)
        {
            try
            {
                var user = _context.User.First(x =>
                    (x.Identifiant.ToLower().Equals(model.Identifiant.ToLower()) ||
                     x.Mail.ToLower().Equals(model.Identifiant)) && x.Password.Equals(model.Password));
                var userModel = new UserModel()
                {
                    Identifiant = user.Identifiant,
                    Mail = user.Mail,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IdUser = user.IdUser,
                    TypeId = user.IdUser
                };
                if (user.CompanyName != null) userModel.CompanyName = user.CompanyName;
                var type = _context.Type.Find(user.TypeId);
                userModel.UserType = new TypeModel() {TypeName = type.TypeName, IdType = type.IdType};
                return userModel;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public bool SelectUser(string projectId, string userId)
        {
            try
            {
                var application = _context.Application
                    .First(x => x.ProjectId.Equals(projectId) && x.UserId.Equals(userId));
                application.IsAssignedTo = 1;
                return _context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public List<UserModel> GetApplicants(string projectId)
        {
            try
            {
                var applications = _context.Application.Where(x => x.ProjectId.Equals(projectId)).ToList();

                return (from application in applications
                    let user = _context.User.Find(application.UserId)
                    select new UserModel
                    {
                        IdUser = user.IdUser, FirstName = user.FirstName, LastName = user.LastName,
                        IsAssignedTo = application.IsAssignedTo.Equals(1)
                    }).ToList();
            }
            catch
            {
                return new List<UserModel>();
            }
        }

        public bool Register(RegisterModel model)
        {
            IDbContextTransaction transaction = null;
            try
            {
                transaction = _context.Database.BeginTransaction();
                var user = new Entities.User
                {
                    Identifiant = model.Email,
                    Mail = model.Email,
                    Password = Encryptor.HashPassword(model.Password),
                    CompanyName = model.CompanyName,
                    FirstName = model.FirstName,
                    IdUser = Guid.NewGuid().ToString(),
                    LastConnected = DateTime.Now,
                    LastName = model.LastName,
                    TypeId = model.UserType
                };
                _context.User.Add(user);
                _context.SaveChanges();

                foreach (var competence in model.Competences)
                {
                    var userCompetence = new UserCompetence
                    {
                        CompetenceId = competence.IdCompetence,
                        UserId = user.IdUser
                    };
                    _context.UserCompetence.Add(userCompetence);
                    _context.SaveChanges();
                }
                
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction?.Rollback();
                return false;
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        public int CountUser(string type)
        {
            try
            {
                var userType = _context.Type.First(x => x.TypeName.ToLower().Equals(type.ToLower()));
                return _context.User.Count(x => x.TypeId.Equals(userType.IdType));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int CountOffers()
        {
            try
            {
                return _context.Project.Count();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<Entities.User> GetUsers(string type, string nom, int page, int affichage)
        {
            try
            {
                var userType = _context.Type.First(x => x.TypeName.Equals(type));
                return _context.User
                    .Where(x => (x.FirstName.ToLower().Contains(nom.ToLower()) ||
                                x.LastName.ToLower().Contains(nom.ToLower()) || (x.CompanyName != null && x.CompanyName.ToLower().Contains(nom.ToLower()))) && x.TypeId.Equals(userType.IdType)).Skip((page - 1) * affichage)
                    .Take(affichage).ToList();
            }
            catch (Exception e)
            {
                return new List<Entities.User>();
            }
        }

        public List<Entities.Competence> GetUserCompetences(string userId)
        {
            try
            {
                var userCompetences = _context.UserCompetence.Where(x => x.UserId.Equals(userId)).ToList();
                return userCompetences.Select(userCompetence => _context.Competence.Find(userCompetence.CompetenceId)).ToList();
            }
            catch (Exception e)
            {
                return new List<Entities.Competence>();
            }
        }

        public int CountResult(string nom, string type)
        {
            try
            {
                var userType = _context.Type.First(x => x.TypeName.Equals(type));
                return _context.User.Count(x =>
                    (x.FirstName.ToLower().Contains(nom.ToLower()) || x.LastName.ToLower().Contains(nom.ToLower())) && x.TypeId.Equals(userType.IdType));
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}