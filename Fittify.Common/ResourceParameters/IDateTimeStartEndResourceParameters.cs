using System;

namespace Fittify.Common.ResourceParameters
{
    public interface IDateTimeStartEndResourceParameters : IResourceParameters
    {
        DateTime? FromDateTimeStart { get; set; }
        DateTime? UntilDateTimeEnd { get; set; }
    }
}
