using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private FittifyContext _context;
        public HomeController(FittifyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
    
}
