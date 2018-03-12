using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.Extensions;
using Fittify.Api.OuterFacingModels;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Fittify.Api.Helpers
{
    public class HateoasLinkFactory<TOfmForGet, TId>
        where TOfmForGet : LinkedResourceBase, IEntityUniqueIdentifier<TId>
        where TId : struct
    {
        protected readonly IUrlHelper UrlHelper;
        protected readonly string ShortPascalCasedControllerName;
        protected readonly string ShortCamelCasedControllerName;
        public HateoasLinkFactory(IUrlHelper urlHelper, string controllerName)
        {
            UrlHelper = urlHelper;
            ShortPascalCasedControllerName = controllerName.ToShortPascalCasedControllerNameOrDefault();
            ShortCamelCasedControllerName = controllerName.ToShortCamelCasedOfmForGetNameOrDefault();
        }
        public TOfmForGet CreateLinksForOfmForGet(TOfmForGet categoryOfmForGet)
        {
            categoryOfmForGet.Links.Add(new HateoasLink(UrlHelper.Link("Get" + ShortPascalCasedControllerName + "ById",
                    new { id = categoryOfmForGet.Id }),
                "self",
                "GET"));

            categoryOfmForGet.Links.Add(
                new HateoasLink(UrlHelper.Link("Delete" + ShortPascalCasedControllerName,
                        new { id = categoryOfmForGet.Id }),
                    "delete_" + ShortCamelCasedControllerName,
                    "DELETE"));

            categoryOfmForGet.Links.Add(
                new HateoasLink(UrlHelper.Link("Update" + ShortPascalCasedControllerName,
                        new { id = categoryOfmForGet.Id }),
                    "update_" + ShortCamelCasedControllerName,
                    "PUT"));

            categoryOfmForGet.Links.Add(
                new HateoasLink(UrlHelper.Link("PartiallyUpdate" + ShortPascalCasedControllerName,
                        new { id = categoryOfmForGet.Id }),
                    "partially_update_" + ShortCamelCasedControllerName,
                    "PATCH"));

            return categoryOfmForGet;
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
                new HateoasLink(UrlHelper.Link("Create" + ShortPascalCasedControllerName, new { id = id }),
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

        public IEnumerable<HateoasLink> CreateLinksForOfmForGetCollection(
            IResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmGetCollectionIncludeByNameSearch(
            ISearchQueryResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            return links;
        }
    }
}
