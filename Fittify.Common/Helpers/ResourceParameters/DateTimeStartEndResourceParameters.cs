using System;

namespace Fittify.Common.Helpers.ResourceParameters
{
    public class DateTimeStartEndResourceParameters : Base, IDateTimeStartEndResourceParameters
    {
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
    }
}
