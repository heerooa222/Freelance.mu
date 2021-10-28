using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Freelancer.Entities;
using Services.Freelancer.Models;

namespace Services.Freelancer.DBProvider.Project
{
    public class ProjectDataProvider : IProjectDataProvider
    {
        private readonly DatabaseContext _context;

        public ProjectDataProvider(DatabaseContext context)
        {
            _context = context;
        }

        public bool Apply(string projectId, string userId)
        {
            try
            {
                var application = new Application
                {
                    ApplicationDate = DateTime.Now,
                    ProjectId = projectId,
                    UserId = userId,
                    IsAssignedTo = 0,
                    IdApplication = Guid.NewGuid().ToString().ToUpper()
                };
                _context.Application.Add(application);
                return _context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool IsAlreadyApplied(string userId, string projetId)
        {
            try
            {
                return _context.Application.Count(x => x.ProjectId.Equals(projetId) && x.UserId.Equals(userId)) > 0;
            }
            catch
            {
                return false;
            }
        }

        public List<ProjectModel> GetProjects(string? keyword, int affichage, int page)
        {
            try
            {
                var motCle = keyword ?? string.Empty;
                var listProjects = _context.Project
                    .Where(x =>
                                x.Title.ToLower().Contains(motCle.ToLower())).Skip((page - 1) * affichage)
                    .Take(affichage).ToList();

                return (from project in listProjects
                    let projectCompetences = _context.ProjectCompetence.Where(x => x.ProjectId.Equals(project.IdProject)).ToList()
                    let projectCompetenceModels = projectCompetences.Select(projectCompetence => _context.Competence.Find(projectCompetence.CompetenceId)).Select(competence => new CompetenceModel() {CompetenceName = competence.CompetenceName, IdCompetence = competence.IdCompetence}).ToList()
                    let user = _context.User.Find(project.AddedBy)
                    select new ProjectModel
                    {
                        Date = project.Date,
                        Description = project.Description,
                        Price = project.Price,
                        Title = project.Title,
                        AddedBy = project.AddedBy,
                        ApplyBefore = project.ApplyBefore,
                        CompetenceRequis = projectCompetenceModels,
                        IdProject = project.IdProject,
                        IsOpen = _context.Application.Count(x => x.ProjectId.Equals(project.IdProject) && x.IsAssignedTo.Equals(1)) == 0 && project.ApplyBefore.CompareTo(DateTime.Now) > 0,
                        AddedByUser = new UserModel {CompanyName = user.CompanyName, FirstName = user.FirstName, LastName = user.LastName}
                    }).ToList();
            }
            catch (Exception e)
            {
                return new List<ProjectModel>();
            }
        }

        public List<ProjectModel> GetMyApplication(string userId, int affichage, int page)
        {
            try
            {
                var myApplications = _context.Application.Where(x => x.UserId.Equals(userId))
                    .OrderByDescending(x => x.ApplicationDate).Skip((page - 1) * affichage).Take(affichage).ToList();

                return (from myApplication in myApplications
                    let project = _context.Project.Find(myApplication.ProjectId)
                    let user = _context.User.Find(project.AddedBy)
                    let isOpen = !_context.Application.Any(x => x.ProjectId.Equals(project.IdProject) && x.IsAssignedTo.Equals(1)) && project.ApplyBefore.CompareTo(DateTime.Now) > 0
                    let isAssignedToMe = myApplication.UserId.Equals(userId) && myApplication.IsAssignedTo.Equals(1)
                    let status = isOpen ? "Sent" : ""
                    let assignedToMe = isAssignedToMe ? "Retained" : "Not selected"
                    let myStatus = string.IsNullOrEmpty(status) ? assignedToMe : status
                    select new ProjectModel
                    {
                        Date = project.Date,
                        Description = project.Description,
                        Price = project.Price,
                        Title = project.Title,
                        AddedBy = project.AddedBy,
                        ApplyBefore = myApplication.ApplicationDate,
                        IdProject = project.IdProject,
                        IsOpen = isOpen,
                        AddedByUser = new UserModel {CompanyName = user.CompanyName, FirstName = user.FirstName, LastName = user.LastName},
                        MyStatus = myStatus
                    }).ToList();
            }
            catch
            {
                return new List<ProjectModel>();
            }
        }

        public int CountMyApplication(string userId)
        {
            try
            {
                return _context.Application.Count(x => x.UserId.Equals(userId));
            }
            catch
            {
                return 0;
            }
        }

        public int CountMyProjects(string userIdUser)
        {
            try
            {
                return _context.Project.Count(x => x.AddedBy.Equals(userIdUser));
            }
            catch
            {
                return 0;
            }
        }

        public List<ProjectModel> GetMyProjects(string userIdUser, int affichage, int page)
        {
            try
            {
                var listProjects = _context.Project
                    .Where(x => x.AddedBy.Equals(userIdUser)).Skip((page - 1) * affichage)
                    .Take(affichage).ToList();

                return (from project in listProjects
                    let projectCompetences = _context.ProjectCompetence.Where(x => x.ProjectId.Equals(project.IdProject)).ToList()
                    select new ProjectModel
                    {
                        Date = project.Date,
                        Description = project.Description,
                        Price = project.Price,
                        Title = project.Title,
                        AddedBy = project.AddedBy,
                        ApplyBefore = project.ApplyBefore,
                        NombreCandidatures = _context.Application.Count(x => x.ProjectId.Equals(project.IdProject)),
                        IdProject = project.IdProject,
                        IsOpen = _context.Application.Count(x => x.ProjectId.Equals(project.IdProject) && x.IsAssignedTo.Equals(1)) == 0 && project.ApplyBefore.CompareTo(DateTime.Now) > 0
                    }).ToList();
            }
            catch
            {
                return new List<ProjectModel>();
            }
        }

        public int CountProjects(string? keyword)
        {
            var motcle = keyword ?? string.Empty;
            try
            {
                return _context.Project.Count(x => x.Title.ToLower().Contains(motcle.ToLower()));
            }
            catch
            {
                return 0;
            }
        }

        public List<ProjectModel> GetLastOffers()
        {
            try
            {
                var projects = _context.Project.OrderByDescending(x => x.Date).Take(6).ToList();

                return (from project in projects
                    let user = _context.User.Find(project.AddedBy)
                    let isOpen = _context.Application.Count(x => x.ProjectId.Equals(project.IdProject) && x.IsAssignedTo.Equals(1)) == 0 && project.ApplyBefore.CompareTo(DateTime.Now) > 0
                    select new ProjectModel()
                    {
                        IdProject = project.IdProject,
                        Date = project.Date,
                        Description = project.Description,
                        Price = project.Price,
                        Title = project.Title,
                        AddedBy = $"{user.FirstName} {user.LastName}",
                        ApplyBefore = project.ApplyBefore,
                        IsOpen = isOpen
                    }).ToList();
            }
            catch (Exception e)
            {
                return new List<ProjectModel>();
            }
        }

        public bool SaveJob(ProjectModel model)
        {
            IDbContextTransaction transaction = null;
            try
            {
                transaction = _context.Database.BeginTransaction();
                var project = new Entities.Project()
                {
                    Date = DateTime.Now,
                    Description = model.Description,
                    Price = model.Price,
                    Title = model.Title,
                    AddedBy = model.AddedBy,
                    IdProject = Guid.NewGuid().ToString().ToUpper(),
                    ApplyBefore = model.ApplyBefore
                };
                _context.Project.Add(project);
                _context.SaveChanges();
                foreach (var projectCompetence in model.CompetenceRequis.Select(competence => new ProjectCompetence()
                {
                    CompetenceId = competence.IdCompetence,
                    ProjectId = project.IdProject
                }))
                {
                    _context.ProjectCompetence.Add(projectCompetence);
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

        public ProjectModel GetProjectById(string projetId)
        {
            try
            {
                var project = _context.Project.Find(projetId);
                var creator = _context.User.Find(project.AddedBy);
                var projectCompetences = _context.ProjectCompetence.Where(x => x.ProjectId.Equals(projetId)).ToList();
                var projectCompetenceModels = projectCompetences.Select(projectCompetence => _context.Competence.Find(projectCompetence.CompetenceId)).Select(competence => new CompetenceModel() {CompetenceName = competence.CompetenceName, IdCompetence = competence.IdCompetence}).ToList();
                var user = _context.User.Find(project.AddedBy);
                return new ProjectModel
                {
                    Date = project.Date,
                    Description = project.Description,
                    Price = project.Price,
                    Title = project.Title,
                    AddedBy = $"{creator.FirstName} {creator.LastName}",
                    ApplyBefore = project.ApplyBefore,
                    IsOpen = _context.Application.Count(x => x.ProjectId.Equals(project.IdProject) && x.IsAssignedTo.Equals(1)) == 0 && project.ApplyBefore.CompareTo(DateTime.Now) > 0,
                    IdProject = projetId,
                    CompetenceRequis = projectCompetenceModels,
                    AddedByUser = new UserModel {CompanyName = user.CompanyName, FirstName = user.FirstName, LastName = user.LastName}
                };
            }
            catch
            {
                return null;
            }
        }
    }
}