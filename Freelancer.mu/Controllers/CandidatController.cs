using Microsoft.AspNetCore.Mvc;
using Services.Freelancer.Models.FrontOffice;
using Services.Freelancer.Services.User;
using Services.Freelancer.Utility;

namespace Freelancer.mu.Controllers
{
    public class CandidatController : Controller
    {
        private readonly IUserService _userService;

        public CandidatController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Browse(string? nom, string? page)
        {
            if (!int.TryParse(page, out var _page) || _page < 1) _page = 1;
            var nombre = _userService.CountResult(nom ?? "", UserType.FREELANCER);
            var browseCandidatModel = new BrowseCandidatModel()
            {
                Candidats = _userService.GetUserList(UserType.FREELANCER, nom ?? "", _page, 5)
            };
            var param = nom != null ? $"nom={nom}&&page=" : "page=";
            var url = $"/candidat/browse?{param}";
            ViewBag.pagination = PageUtility.MakePagination(5, nombre, _page, url);
            return View(browseCandidatModel);
        }

        [HttpGet]
        public IActionResult Employeur(string? nom, string? page)
        {
            if (!int.TryParse(page, out var _page) || _page < 1) _page = 1;
            var nombre = _userService.CountResult(nom ?? "", UserType.EMPLOYER);
            var browseCandidatModel = new BrowseCandidatModel()
            {
                Candidats = _userService.GetUserList(UserType.EMPLOYER, nom ?? "", _page, 5)
            };
            var param = nom != null ? $"nom={nom}&&page=" : "page=";
            var url = $"/candidat/employeur?{param}";
            ViewBag.pagination = PageUtility.MakePagination(5, nombre, _page, url);
            return View(browseCandidatModel);
        }
    }
}