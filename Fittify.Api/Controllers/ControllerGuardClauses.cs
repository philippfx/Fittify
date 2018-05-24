using System.Linq;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Common.Extensions;
using Fittify.Common.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers
{
    public sealed class ControllerGuardClauses<TOfmForGet, TOfmForPost, TOfmForPatch, TId> : IControllerGuardClauses<TOfmForGet, TOfmForPost, TId> where TOfmForGet : class
        where TOfmForPost : class
        where TOfmForPatch : class 
        where TId : struct
    {
        private readonly Controller _controller;
        private readonly string _shortCamelCasedControllerName;
        public ControllerGuardClauses(Controller controller)
        {
            _controller = controller;
            _shortCamelCasedControllerName = controller.GetType().Name.ToShortCamelCasedControllerName();
        }
        public bool ValidateGetCollection(
            OfmForGetCollectionQueryResult<TOfmForGet> ofmForGetCollectionQueryResult,
            out ObjectResult objectResult)
        {
            objectResult = null;
            if (ofmForGetCollectionQueryResult.ErrorMessages.Count() > 0)
            {
                foreach (var errorMessage in ofmForGetCollectionQueryResult.ErrorMessages)
                {
                    _controller.ModelState.AddModelError(_shortCamelCasedControllerName, errorMessage);
                }
                objectResult = new UnprocessableEntityObjectResult(_controller.ModelState);
                return false;
            }

            var allOfmForGet = ofmForGetCollectionQueryResult.ReturnedTOfmForGetCollection;
            if (allOfmForGet.OfmForGets.Count() == 0)
            {
                _controller.ModelState.AddModelError(_shortCamelCasedControllerName, $"No {_shortCamelCasedControllerName.ToPlural()} found");
                objectResult = new EntityNotFoundObjectResult(_controller.ModelState);
                return false;
            }

            return true;
        }

        public bool ValidateGetById(
            OfmForGetQueryResult<TOfmForGet> ofmForGetQueryResult,
            int id,
            out ObjectResult objectResult)
        {
            objectResult = null;
            if (ofmForGetQueryResult.ErrorMessages.Count > 0)
            {
                foreach (var errorMessage in ofmForGetQueryResult.ErrorMessages)
                {
                    _controller.ModelState.AddModelError(_shortCamelCasedControllerName, errorMessage);
                }
                objectResult = new UnprocessableEntityObjectResult(_controller.ModelState);
                return false;
            }

            //var entity = await _repo.GetById(id);
            if (ofmForGetQueryResult.ReturnedTOfmForGet == null)
            {
                _controller.ModelState.AddModelError(_shortCamelCasedControllerName, "No " + _shortCamelCasedControllerName + " found for id=" + id);
                objectResult = new EntityNotFoundObjectResult(_controller.ModelState);
                return false;
            }
            return true;
        }

        public bool ValidatePost(
            TOfmForPost ofmForPost,
            out ObjectResult objectResult)
        {
            objectResult = null;
            if (ofmForPost == null)
            {
                _controller.ModelState.AddModelError(_shortCamelCasedControllerName, "The supplied body for the " + _shortCamelCasedControllerName + " is null.");
                objectResult = new BadRequestObjectResult(_controller.ModelState);
                return false;
            }

            if (!_controller.ModelState.IsValid)
            {
                objectResult = new UnprocessableEntityObjectResult(_controller.ModelState);
                return false;
            }
            return true;
        }

        public bool ValidateDelete(
            OfmDeletionQueryResult<TId> ofmDeletionQueryResult, 
            TId id,
            out ObjectResult objectResult)
        {
            objectResult = null;

            if (ofmDeletionQueryResult.ErrorMessages.Count > 0)
            {
                // For the moment, we return 500 error message to avoid exposing too much info.
                // No errorMessage from lower levels are anticipated, so an exception was probably thrown
                _controller.ModelState.AddModelError(_shortCamelCasedControllerName, "There was an internal server error. Please contact support.");
                objectResult = new InternalServerErrorObjectResult(_controller.ModelState);
                return false;
            }

            if (ofmDeletionQueryResult.DidEntityExist == false && ofmDeletionQueryResult.ErrorMessages.Count == 0)
            {
                _controller.ModelState.AddModelError(_shortCamelCasedControllerName, "No " + _shortCamelCasedControllerName + " found for id=" + id);
                objectResult = new EntityNotFoundObjectResult(_controller.ModelState);
                return false;
            }

            return true;
        }
    }
}
