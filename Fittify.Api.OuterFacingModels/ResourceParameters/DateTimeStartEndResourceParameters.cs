using System;

namespace Fittify.Api.OuterFacingModels.ResourceParameters
{
    public class DateTimeStartEndResourceParameters : Base, IDateTimeStartEndResourceParameters
    {
        public DateTime? FromDateTimeStart { get; set; }
        public DateTime? UntilDateTimeEnd { get; set; }
    }
}
