using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Controllers.Sport;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.OfmFactory;
using Fittify.Api.OfmRepository;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd.Sport;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.Services;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common;
using Fittify.DataModelRepository;
using Fittify.DataModelRepository.Repository.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fittify.Api.Controllers.Generic
{
    [Route("[controller]")]
    [GenericControllerNameAttribute]
    public class GenericController<T> : Controller where T : class
    {
        //private readonly IAsyncGppd<CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int, CategoryOfmResourceParameters> _asyncGppd;
        //private readonly AsyncGppdFactory<AnimalOfmRepository> _asyncGppd;
        private readonly AnimalOfmRepository _animalOfmRepository;
        private readonly string _shortCamelCasedControllerName;
        private readonly IUrlHelper _urlHelper;
        private readonly ControllerGuardClauses<CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int> _controllerGuardClause;
        private readonly HateoasLinkFactory<int> _hateoasLinkFactory;
        private readonly IncomingHeaders _incomingHeaders;

        public GenericController(
            //IAsyncGppd<CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int, CategoryOfmResourceParameters> asyncGppd,
            //IAsyncGppd<T> asyncGppd,
            IServiceProvider serviceProvier,
            IUrlHelper urlHelper,
            IHttpContextAccessor httpContextAccesor)
        {
            var _asyncGppd = new AsyncGppdFactory<AnimalOfmRepository>(serviceProvier);
            _animalOfmRepository = _asyncGppd.CreateGeneric<Fittify.Api.Controllers.Generic.Animals>();
            _shortCamelCasedControllerName = nameof(CategoryApiController).ToShortCamelCasedControllerName();
            _urlHelper = urlHelper;
            _controllerGuardClause = new ControllerGuardClauses<CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int>(this);
            _hateoasLinkFactory = new HateoasLinkFactory<int>(_urlHelper, nameof(CategoryApiController));
            _incomingHeaders = Mapper.Map<IncomingHeaders>(httpContextAccesor.HttpContext.Items[nameof(IncomingRawHeaders)] as IncomingRawHeaders);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Content($"GET from a {typeof(T).Name} controller.");
        }

        [HttpGet("data")]
        public async Task<IActionResult> GetData()
        {
            var result = await _animalOfmRepository.GetCollection(new AnimalOfmResourceParameters(), Guid.NewGuid());
            return new JsonResult(result);
        }
    }
}
