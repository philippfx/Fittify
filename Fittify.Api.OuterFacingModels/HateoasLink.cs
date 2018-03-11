using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Api.OuterFacingModels
{
    public class HateoasLink
    {
        public string Href { get; private set; }
        public string Rel { get; private set; }
        public string Method { get; private set; }

        public HateoasLink(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
