using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.View.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
