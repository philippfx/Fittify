﻿namespace Fittify.Common.ResourceParameters
{
    public interface IResourceParameters
    {
        string Ids { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string Fields { get; set; }
    }
}