using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fittify.Api.OuterFacingModels
{
    public class OfmForGetCollection<TOfmForGet> where TOfmForGet : class
    {
        public IEnumerable<TOfmForGet> OfmForGets { get; set; }
    }
}
