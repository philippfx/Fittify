using System;

namespace Fittify.Api.OuterFacingModels.ResourceParameters
{
    public interface IDateTimeStartEndResourceParameters : IResourceParameters
    {
        DateTime? FromDateTimeStart { get; set; }
        DateTime? UntilDateTimeEnd { get; set; }
    }
}
