using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModelRepository;
using Fittify.Client.ViewModelRepository.Sport;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("mapexerciseworkout")]
    public class MapExerciseWorkoutController : Controller
    {
        private readonly IViewModelRepository<int, MapExerciseWorkoutViewModel, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmResourceParameters, MapExerciseWorkoutOfmCollectionResourceParameters> _mapExerciseWorkoutViewModelRepository;

        public MapExerciseWorkoutController(
            IViewModelRepository<int, MapExerciseWorkoutViewModel, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmResourceParameters, MapExerciseWorkoutOfmCollectionResourceParameters> mapExerciseWorkoutViewModelRepository)
        {
            _mapExerciseWorkoutViewModelRepository = mapExerciseWorkoutViewModelRepository;
        }
        
        [HttpPost]
        [Route("deletion")]
        //public async Task<RedirectToActionResult> Delete([FromQuery] int workoutId, [FromQuery] int exerciseId)
        public async Task<RedirectToActionResult> Delete([FromQuery] int mapExericseWorkoutId, [FromQuery] int workoutId)
        {
            ////var getMapExerciseWorkoutViewModelResult = await _mapExerciseWorkoutViewModelRepository.GetCollection(new MapExerciseWorkoutOfmCollectionResourceParameters()
            ////{
            ////    ExerciseId = exerciseId,
            ////    WorkoutId = workoutId
            ////});

            ////if (getMapExerciseWorkoutViewModelResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
            ////    getMapExerciseWorkoutViewModelResult.HttpStatusCode == HttpStatusCode.Forbidden)
            ////{
            ////    return RedirectToAction("AccessDenied", "Authorization");
            ////}

            //if ((int)getMapExerciseWorkoutViewModelResult.HttpStatusCode != 200)
            //{
            //    // Todo: Do something when getting failed
            //}

            ////var deleteMapExerciseWorkoutViewModelResult = await _mapExerciseWorkoutViewModelRepository.Delete(
            ////    getMapExerciseWorkoutViewModelResult.ViewModelForGetCollection.First().Id);

            var deleteMapExerciseWorkoutViewModelResult =
                await _mapExerciseWorkoutViewModelRepository.Delete(mapExericseWorkoutId);

            if (deleteMapExerciseWorkoutViewModelResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                deleteMapExerciseWorkoutViewModelResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            //if ((int)deleteMapExerciseWorkoutViewModelResult.HttpStatusCode != 204)
            //{
            //    // Todo: Do something when deleting failed
            //}

            return RedirectToAction("AssociatedExercises", "Workout", new { workoutId = workoutId });
        }
    }
}