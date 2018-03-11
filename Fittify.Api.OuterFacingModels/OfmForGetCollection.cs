using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fittify.Api.OuterFacingModels
{
    public class OfmForGetCollection<TOfmForGet> where TOfmForGet : LinkedResourceBase
    {
        public IEnumerable<TOfmForGet> OfmForGets { get; set; }
        public IEnumerable<HateoasLink> HateoasLinks { get; set; }
    }
}
