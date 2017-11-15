using System.Linq;
using Fittify.DataModelRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.Controllers
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