using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fittify.Api.OuterFacingModels
{
    public abstract class LinkedResourceBase
    {
        public List<HateoasLink> Links { get; set; } = new List<HateoasLink>();
    }
}
