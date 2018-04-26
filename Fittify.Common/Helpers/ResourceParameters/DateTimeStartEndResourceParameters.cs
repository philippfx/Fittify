using System;

namespace Fittify.Common.Helpers.ResourceParameters
{
    public class DateTimeStartEndResourceParameters : Base, IDateTimeStartEndResourceParameters
    {
        public DateTime? FromDateTimeStart { get; set; }
        public DateTime? UntilDateTimeEnd { get; set; }
    }
}
