using System;

namespace Fittify.Common.Helpers.ResourceParameters
{
    public interface IDateTimeStartEndResourceParameters : IResourceParameters
    {
        DateTime? DateTimeStart { get; set; }
        DateTime? DateTimeEnd { get; set; }
    }
}
