using System.Linq;
using Fittify.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Controllers
{
    public class WorkoutController : Controller
    {
        private IWorkoutSession _trainingSessions;

        public WorkoutController(IWorkoutSession trainingSessions)
        {
            _trainingSessions = trainingSessions;
        }
        //public IActionResult Index()
        //{
        //    var model = _trainingSessions.GetAll().OrderByDescending(p => p.SessionEnd).ToList();

        //    return View(model);

        //}

        public IActionResult Details(int id)
        {
            var model = _trainingSessions.Get(id);
            return View(model);
        }
    }
}
