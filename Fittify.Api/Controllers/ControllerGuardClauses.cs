using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.Extensions;
using Fittify.Api.Helpers;
using Fittify.Api.OuterFacingModels;
using Fittify.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers
{
    public class ControllerGuardClauses<TOfmForGet> where TOfmForGet : LinkedResourceBase
    {
        private Controller _controller;
        private string _shortCamelCasedControllerName;
        public ControllerGuardClauses(Controller controller)
        {
            _controller = controller;
            _shortCamelCasedControllerName = controller.GetType().Name.ToShortCamelCasedControllerNameOrDefault();
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
                objectResult = new UnprocessableEntityObjectResult(_controller.ModelState);
                return false;
            }
            return true;
        }
    }
}
