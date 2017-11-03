using System.Linq;
using Fittify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Fittify.Controllers
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
            var result = _context.CardioSets.ToList();
            return View();
        }
    }
}
