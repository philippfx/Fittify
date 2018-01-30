using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.View.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
    
}
