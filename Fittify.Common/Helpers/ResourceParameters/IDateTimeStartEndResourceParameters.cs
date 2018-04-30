using System;

namespace Fittify.Common.Helpers.ResourceParameters
{
    public interface IDateTimeStartEndResourceParameters : IResourceParameters
    {
        DateTime? FromDateTimeStart { get; set; }
        DateTime? UntilDateTimeEnd { get; set; }
    }
}
