using System;
using System.Collections.Generic;
using System.Text;using Fittify.Common.Helpers.ResourceParameters;

namespace Fittify.Common.Helpers.ResourceParameters
{
    public interface ISearchQueryResourceParameters : IResourceParameters
    {
        string SearchQuery { get; set; }
    }
}
