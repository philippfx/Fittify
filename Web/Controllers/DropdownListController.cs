using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class DropdownListController : Controller
    {
        FittifyContext _fittifyContext;
        public DropdownListController(FittifyContext fittifyContext)
        {
            _fittifyContext = fittifyContext;
        }
        public IActionResult Index()
        {
            var model = _fittifyContext.Exercises.ToList();
            return View(model);
        }
    }
}