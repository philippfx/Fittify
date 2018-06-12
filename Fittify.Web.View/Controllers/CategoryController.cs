using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModelRepository;
using Fittify.Client.ViewModelRepository.Sport;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("categories")]
    public class CategoryController : Controller
    {
        private readonly IViewModelRepository<int, CategoryViewModel, CategoryOfmForPost, CategoryOfmResourceParameters, CategoryOfmCollectionResourceParameters> _categoryViewModelRepository;
        public CategoryController(
            IViewModelRepository<int, CategoryViewModel, CategoryOfmForPost, CategoryOfmResourceParameters, CategoryOfmCollectionResourceParameters> categoryViewModelRepository)
        {
            _categoryViewModelRepository = categoryViewModelRepository;
        }

        public async Task<IActionResult> Overview()
        {
            var categoryViewModelCollectionResult = await _categoryViewModelRepository.GetCollection(new CategoryOfmCollectionResourceParameters());

            if (categoryViewModelCollectionResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                categoryViewModelCollectionResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            //if ((int)categoryViewModelCollectionResult.HttpStatusCode != 200)
            //{
            //    // Todo: Do something when posting failed
            //}

            return View("Overview", categoryViewModelCollectionResult.ViewModelForGetCollection.ToList());
        }

        //[HttpPost]
        //public async Task<RedirectToActionResult> CreateCategory([FromForm] CategoryOfmForPost categoryOfmForPost, [FromQuery] int workoutId)
        //{
        //    var postResult = await _categoryViewModelRepository.Create(categoryOfmForPost);

        //    if (postResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
        //        postResult.HttpStatusCode == HttpStatusCode.Forbidden)
        //    {
        //        return RedirectToAction("AccessDenied", "Authorization");
        //    }

        //    if ((int)postResult.HttpStatusCode != 201)
        //    {
        //        // Todo: Do something when posting failed
        //    }

        //    return RedirectToAction("Overview", "Category", new { workoutId = workoutId });
        //}

        //[HttpPost]
        //[Route("{id}/deletion")]
        //public async Task<RedirectToActionResult> Delete(int id)
        //{
        //    var deleteResult = await _categoryViewModelRepository.Delete(id);

        //    if (deleteResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
        //        deleteResult.HttpStatusCode == HttpStatusCode.Forbidden)
        //    {
        //        return RedirectToAction("AccessDenied", "Authorization");
        //    }

        //    if ((int)deleteResult.HttpStatusCode != 204)
        //    {
        //        // Todo: Do something when deleting failed
        //    }

        //    return RedirectToAction("Overview", "CategoryResourceParameters", null);
        //}

        //[HttpPost]
        //[Route("{id}/patch")]
        //public async Task<RedirectToActionResult> PatchName(int id, [FromForm] CategoryOfmForPatch categoryOfmForPatch)
        //{
        //    JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();

        //    jsonPatchDocument.Replace("/" + nameof(categoryOfmForPatch.Name), categoryOfmForPatch.Name);

        //    var patchResult = await _categoryViewModelRepository.PartiallyUpdate(id, jsonPatchDocument);

        //    if (patchResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
        //        patchResult.HttpStatusCode == HttpStatusCode.Forbidden)
        //    {
        //        return RedirectToAction("AccessDenied", "Authorization");
        //    }

        //    if ((int)patchResult.HttpStatusCode != 200)
        //    {
        //        // Todo: Do something when posting failed
        //    }

        //    return RedirectToAction("Overview", "CategoryResourceParameters", null);
        //}
    }
}
