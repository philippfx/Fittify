using System;
using System.Collections.Generic;
using AutoMapper;
using Fittify.Api.Controllers.Sport;
using Fittify.Api.Helpers;
using Fittify.Api.OfmRepository;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.Services;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Repository.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Fittify.Api.Controllers.Generic
{
    [Route("[controller]")]
    [GenericControllerNameAttribute]
    public class GenericController<T> : Controller where T : IEntityName<int>
    {
        //private readonly AsyncPostPatchDeleteOfm<CategoryRepository, Category, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int> _asyncPostPatchDeleteForHttpMethods;
        //private readonly IAsyncGetOfmById<CategoryOfmForGet, int> _asyncGetOfmById;
        private readonly IAsyncGetOfmCollectionByNameSearch<CategoryOfmForGet> _asyncGetOfmCollectionIncludeByNameSearch;
        private readonly CategoryRepository _repo;
        private readonly string _shortCamelCasedControllerName;
        private ITypeHelperService _typeHelperService;
        private readonly IUrlHelper _urlHelper;
        private readonly ControllerGuardClauses<CategoryOfmForGet> controllerGuardClause;
        private readonly HateoasLinkFactory<int> _hateoasLinkFactory;
        private readonly IConfiguration _appConfiguration;
        private readonly IncomingHeaders _incomingHeaders;
        private readonly IActionDescriptorCollectionProvider _adcp;
        private readonly IPropertyMappingService _propertyMappingService;

        public GenericController(FittifyContext fittifyContext,
            IActionDescriptorCollectionProvider adcProvider,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            IConfiguration appConfiguration,
            IHttpContextAccessor httpContextAccesor)
        {
            _repo = new CategoryRepository(fittifyContext);
            //_asyncPostPatchDeleteForHttpMethods = new AsyncPostPatchDeleteOfm<CategoryRepository, Category, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int>(_repo, urlHelper, adcProvider, this);
            //_shortCamelCasedControllerName = nameof(CategoryApiController).ToShortCamelCasedControllerNameOrDefault();
            //_asyncGetOfmById = new AsyncGetOfmCollectionIncludeByNameSearch<CategoryRepository, Category, CategoryOfmForGet, int>(_repo, urlHelper, adcProvider, propertyMappingService, typeHelperService, this);
            //_asyncGetOfmCollectionIncludeByNameSearch = new AsyncGetOfmCollectionIncludeByNameSearch<CategoryRepository, Category, CategoryOfmForGet, int>(_repo, urlHelper, adcProvider, propertyMappingService, typeHelperService, this);
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            controllerGuardClause = new ControllerGuardClauses<CategoryOfmForGet>(this);
            _hateoasLinkFactory = new HateoasLinkFactory<int>(urlHelper, nameof(CategoryApiController));
            _appConfiguration = appConfiguration;
            //_incomingHeaders = Mapper.Map<IncomingHeaders>(incomingRawHeaders)
            _incomingHeaders = Mapper.Map<IncomingHeaders>(httpContextAccesor.HttpContext.Items[nameof(IncomingRawHeaders)] as IncomingRawHeaders);
            _adcp = adcProvider;
            _propertyMappingService = propertyMappingService;
        }

        [HttpGet]
        public IActionResult IndexAsync()
        {
            return Content($"GET from a {typeof(T).Name} controller.");
        }

        [HttpGet("advanced")]
        public IActionResult Create([FromBody] IEnumerable<T> items)
        {
            // AsyncGetOfmById<TCrudRepository, TEntity, TOfmForGet, TId>
            var d1 = typeof(AsyncGetOfmById<,,,>);
            Type[] typeArgs = { typeof(CategoryRepository), typeof(Category), typeof(CategoryOfmForGet), typeof(int) };
            var makeme = d1.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(makeme, _repo, _urlHelper, _adcp, _propertyMappingService, _typeHelperService, this);

            //var result = (AsyncGetOfmById<CategoryRepository, Category, CategoryOfmForGet, int>) o;
            var result = (IAsyncGetOfmById<CategoryOfmForGet, int>)o;

            // BIG QUESTION IS HOW TO CAST GENERIC CLASS TO CONCRETE INSTANTIATED CLASS 

            
            

            var myGenericObject = Activator.CreateInstance(typeof(AsyncCrud<,>).MakeGenericType(typeArgs));

            return Content($"GET from a {typeof(T).Name} controller.");
        }
    }
}
