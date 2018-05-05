using System.Linq;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers
{
    public sealed class ControllerGuardClauses<TOfmForGet> where TOfmForGet : class
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
    }
}
