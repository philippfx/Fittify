using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fittify.Api.Helpers;
using Fittify.Api.OuterFacingModels;
using Fittify.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Fittify.Api.Controllers
{
    [Route("api")]
    public class RootController : Controller
    {
        private readonly IUrlHelper _urlHelper;
        private readonly IActionDescriptorCollectionProvider _provider;
        private readonly IConfiguration _appConfiguration;
        private readonly IncomingHeaders _incomingHeaders;

        public RootController(IUrlHelper urlHelper,
            IActionDescriptorCollectionProvider adcProvider,
            IConfiguration appConfiguration,
            IHttpContextAccessor httpContextAccesor)
        {
            _provider = adcProvider;
            _urlHelper = urlHelper;
            _appConfiguration = appConfiguration;
            _incomingHeaders = Mapper.Map<IncomingHeaders>(httpContextAccesor.HttpContext.Items[nameof(IncomingRawHeaders)] as IncomingRawHeaders);
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            //if (!incomingRawHeaders.Validate(_appConfiguration, out var errorMessages)) return BadRequest(new Dictionary<string, List<string>>() { { "header", errorMessages } });
            //var incomingHeaders = Mapper.Map<IncomingHeaders>(incomingRawHeaders);

            var routes = _provider.ActionDescriptors.Items
                .Select(x => new
                {
                    Action = x.RouteValues["Action"],
                    Controller = x.RouteValues["Controller"],
                    Name = x.AttributeRouteInfo.Name,
                    Template = x.AttributeRouteInfo.Template
                })
                .ToList();
            var getCollectionActions = _provider.ActionDescriptors.Items
                .Where(w => w.RouteValues.Values.FirstOrDefault(f => f.Contains("GetPagedCollection")) != null).ToList();
            
            var links = new List<HateoasLink>();
            links.Add(new HateoasLink(_urlHelper.Link("GetRoot", null), "self", "GET"));

            foreach (var getCollectionAction in getCollectionActions)
            {
                string abbreviatedControllerName = getCollectionAction.RouteValues["controller"].Replace("Api", "");
                links.Add(new HateoasLink(_urlHelper.Link("Get" + abbreviatedControllerName + "Collection", null),
                    abbreviatedControllerName.ToLower().ToPlural(),
                    "GET"));
                links.Add(new HateoasLink(_urlHelper.Link("CreateAsync" + abbreviatedControllerName, null),
                    "create_" + abbreviatedControllerName.ToLower(),
                    "POST"));
            }
            
            return Ok(links);
        }
    }
}
