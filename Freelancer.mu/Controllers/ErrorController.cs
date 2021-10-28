using Microsoft.AspNetCore.Mvc;

namespace Freelancer.mu.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Code500()
        {
            return View("InternalServerError");
        }
        
        public ActionResult Code404()
        {
            return View("NotFound");
        }
        
        public ActionResult Code403()
        {
            return View("AccessDenied");
        }
    }
}