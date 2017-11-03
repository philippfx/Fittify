using Fittify.Services;
using Fittify.Services.MockData;
using Fittify.Services.MockData.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Controllers
{
    public class PracticeController : Controller
    {
        private IWorkoutSessionViewModelData _trainingSessionViewModelMockData;

        public PracticeController(IWorkoutSessionViewModelData trainingSessionViewModelData)
        {
            _trainingSessionViewModelMockData = trainingSessionViewModelData;
        }
        public IActionResult Index()
        {
            var model = _trainingSessionViewModelMockData.GetFirstOrDefault();

            return View(model);

            //return new ObjectResult(model); // returns a .json file by default

            //return Content("Hello from PracticeController!");
        }
    }
}
