using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.View.Controllers
{
    public class AuthorizationController : Controller
    {

        [ExcludeFromCodeCoverage] // dead simple, not need to test
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
