using System.Collections.Generic;
using System.Linq;
using Fittify.Api.OfmRepository.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Helpers
{
    public static class IEnumerableHateoasLinksExtensions
    {
        public static IEnumerable<ExpandableOfmForGet> CreateHateoasForExpandableOfmForGets<TOfmForGet, TId>(
            this IEnumerable<ExpandableOfmForGet> expandableOfmForGetCollection,
            IUrlHelper urlhelper,
            string controllerName,
            string fields)

            where TOfmForGet : class
            where TId : struct
        {
            var hateoasLinkFactory = new HateoasLinkFactory<TId>(urlhelper, controllerName);
            var expandableOfmForGets = new List<ExpandableOfmForGet>();
            foreach (var expandableOfmForGet in expandableOfmForGetCollection)
            {
                expandableOfmForGet.Add("links", hateoasLinkFactory.CreateLinksForOfmForGet((TId)expandableOfmForGet["Id"], fields).ToList());
                expandableOfmForGets.Add(expandableOfmForGet);
            }

            return expandableOfmForGets;
        }
    }
}
