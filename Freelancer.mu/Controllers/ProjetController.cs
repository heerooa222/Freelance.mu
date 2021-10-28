using System.Linq;
using Freelancer.mu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Freelancer.Models;
using Services.Freelancer.Services.Project;
using Services.Freelancer.Utility;

namespace Freelancer.mu.Controllers
{
    public class ProjetController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjetController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public JsonResult GetApplicants(string projectId)
        {
            var users = _projectService.GetApplicants(projectId);
            var isAlreadyAssigned = users.Any(x => x.IsAssignedTo.Equals(true));
            return Json(new { status = isAlreadyAssigned ? 1 : 0, data = users});
        }

        [HttpPost]
        public JsonResult SelectUser(string projectId, string userId)
        {
            var isSelected = _projectService.SelectUser(projectId, userId);
            return Json(new {status = isSelected ? 1 : 0});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Postuler(string projectId)
        {
            var user = JsonConvert.DeserializeObject<UserModel>(HttpContext.Session.GetString("user"));
            if (!user.UserType.TypeName.Equals(UserType.FREELANCER))
                return Redirect($"/projet/detail?projetId={projectId}&error=is-employer");
            var isApplied = _projectService.Apply(projectId, user.IdUser);
            return Redirect($"/projet/detail?projetId={projectId}&{(isApplied ? "success=true" : "error=true")}");
        }

        [AuthenticationFilter]
        public IActionResult Detail(string projetId, string? error, string? success)
        {
            if (!string.IsNullOrEmpty(error)) ViewBag.error = "An error has occured, please try again!";
            if (!string.IsNullOrEmpty(error) && error.Equals("is-employer")) ViewBag.error = "You cannot apply as employer!";
            if (!string.IsNullOrEmpty(success)) ViewBag.success = "Application successfull!";
            var user = JsonConvert.DeserializeObject<UserModel>(HttpContext.Session.GetString("user"));
            var projetModel = _projectService.GetProjetById(projetId);
            projetModel.AlreadyApplied = _projectService.IsAlreadyApplied(user.IdUser, projetId);
            return View("DetailOffre", projetModel);
        }
        
        public IActionResult Offres(string? page, string? keyword)
        {
            if (!int.TryParse(page, out var _page) || _page < 1) _page = 1;
            var projetModels = _projectService.GetProjects(keyword, 5, _page);
            var nombre = _projectService.CountProjects(keyword);
            ViewBag.pagination = PageUtility.MakePagination(5, nombre, _page, $"/projet/offres?keyword={keyword}&page=");
            return View(projetModels);
        }
        
        [RoleAccessFilter]
        public IActionResult MesOffres(string? page)
        {
            if (!int.TryParse(page, out var _page) || _page < 1) _page = 1;
            var user = JsonConvert.DeserializeObject<UserModel>(HttpContext.Session.GetString("user"));
            var projetModels = _projectService.GetMyProjects(user.IdUser, 5, _page);
            var nombre = _projectService.CountMyProjects(user.IdUser);
            ViewBag.pagination = PageUtility.MakePagination(5, nombre, _page, $"/projet/mesoffres?&page=");
            return View(projetModels);
        }

        [AuthenticationFilter]
        public IActionResult MesCandidatures(string? page)
        {
            var user = JsonConvert.DeserializeObject<UserModel>(HttpContext.Session.GetString("user"));
            if (!int.TryParse(page, out var _page) || _page < 1) _page = 1;
            var myApplication = _projectService.MyApplication(user.IdUser, 5, _page);
            var nombre = _projectService.CountMyApplication(user.IdUser);
            ViewBag.pagination = PageUtility.MakePagination(5, nombre, _page, "/projet/mescandidatures?page=");
            return View(myApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAccessFilter]
        public IActionResult Add(ProjectModel model)
        {
            var user = HttpContext.Session.GetString("user");
            var userModel = JsonConvert.DeserializeObject<UserModel>(user);
            model.AddedBy = userModel.IdUser;
            var isShared = _projectService.PartagerOffre(model);
            return Redirect($"/projet/add?{(isShared ? "success=true" : "error=occured")}");
        }

        [RoleAccessFilter]
        [HttpGet]
        public IActionResult Add(string? error, string? success)
        {
            if (!string.IsNullOrEmpty(error)) ViewBag.error = "An error has occured, please try again!";
            if (!string.IsNullOrEmpty(success)) ViewBag.success = "Shared with success!";
            var projectModel = new ProjectModel
            {
                CompetenceRequis = _projectService.GetAllCompetences()
            };
            return View("Ajouter", projectModel);
        }
    }
}