using System.Collections.Generic;
using Fittify.Api.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Helpers
{
    public class HateoasLinkFactory<TId>
        where TId : struct
    {
        protected readonly IUrlHelper UrlHelper;
        protected readonly string ShortPascalCasedControllerName;
        protected readonly string ShortCamelCasedControllerName;
        public HateoasLinkFactory(IUrlHelper urlHelper, string controllerName)
        {
            UrlHelper = urlHelper;
            ShortPascalCasedControllerName = controllerName.ToShortPascalCasedControllerName();
            ShortCamelCasedControllerName = controllerName.ToShortCamelCasedControllerName();
        }
        
        public IEnumerable<HateoasLink> CreateLinksForOfmForGet(TId id, string fields)
        {
            var links = new List<HateoasLink>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new HateoasLink(UrlHelper.Link("Get" + ShortPascalCasedControllerName + "ById", new { id = id }),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add(
                    new HateoasLink(UrlHelper.Link("Get" + ShortPascalCasedControllerName + "ById", new { id = id, fields = fields }),
                        "self",
                        "GET"));
            }
            
            links.Add(
                new HateoasLink(UrlHelper.Link("CreateAsync" + ShortPascalCasedControllerName, null),
                    "create_" + ShortCamelCasedControllerName,
                    "POST"));

            links.Add(
                new HateoasLink(UrlHelper.Link("PartiallyUpdate" + ShortPascalCasedControllerName,
                        new { id = id }),
                    "partially_update_" + ShortCamelCasedControllerName,
                    "PATCH"));

            links.Add(
                new HateoasLink(UrlHelper.Link("Delete" + ShortPascalCasedControllerName, new { id = id }),
                    "delete_" + ShortCamelCasedControllerName,
                    "DELETE"));


            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmGetGeneric(
            IDictionary<string, object> resourceParametersAsDictionary,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForGeneric(resourceParametersAsDictionary, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));
            
            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForGeneric(resourceParametersAsDictionary, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
                resourceParametersAsDictionary["PageNumber"] = (int) resourceParametersAsDictionary["PageNumber"] - 1; // resetting pageNumber
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForGeneric(resourceParametersAsDictionary, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
                resourceParametersAsDictionary["PageNumber"] = (int)resourceParametersAsDictionary["PageNumber"] + 1; // resetting pageNumber
            }

            return links;
        }
    }
}
