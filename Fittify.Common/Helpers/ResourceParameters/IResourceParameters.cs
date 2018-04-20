using System;

namespace Fittify.Common.Helpers.ResourceParameters
{
    public interface IResourceParameters
    {
        string Ids { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string OrderBy { get; set; }
        string Fields { get; set; }
    }
}