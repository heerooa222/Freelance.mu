using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Freelancer.Models;
using Services.Freelancer.Services.Project;
using Services.Freelancer.Services.Type;
using Services.Freelancer.Services.User;

namespace Freelancer.mu.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITypeService _typeService;
        private readonly IProjectService _projectService;

        public AccountController(IUserService userService, ITypeService typeService, IProjectService projectService)
        {
            _userService = userService;
            _typeService = typeService;
            _projectService = projectService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserModel model)
        {
            if (string.IsNullOrEmpty(model.Next)) model.Next = "/";
            var userModel = _userService.GetUser(model);
            if (userModel == null) return Redirect($"/account/login?invalidlogin=true&next={model.Next}");
            HttpContext.Session.SetString("user", JsonConvert.SerializeObject(userModel));
            return Redirect(model.Next);
        }
        public IActionResult Login(string? invalidLogin, string? next)
        {
            if (!string.IsNullOrEmpty(next)) ViewBag.Next = next;
            if (!string.IsNullOrEmpty(invalidLogin)) ViewBag.error = "Invalid username or password!";
            return View();
        }
        
        public IActionResult Register()
        {
            ViewBag.types = _typeService.GetUserTypes();
            ViewBag.skills = _projectService.GetAllCompetences();
            return View();
        }
        
        [HttpPost]
        public IActionResult SaveUser(RegisterModel model)
        {
            ViewBag.types = _typeService.GetUserTypes();
            ViewBag.skills = _projectService.GetAllCompetences();
            var isSaved = _userService.Register(model);
            if (isSaved)
            {
                var userModel = new UserModel
                {
                    Identifiant = model.Email,
                    Password = model.Password
                };
                var result = _userService.GetUser(userModel);
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(result));
                return Redirect("/");
            }
            else
            {
                ViewBag.error = "Unable to register, please try later";
            }
            return View("Register");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}