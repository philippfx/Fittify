using System;

namespace Fittify.Common.ResourceParameters
{
    public class DateTimeStartEndResourceParameters : BaseResourceParameters, IDateTimeStartEndResourceParameters
    {
        public DateTime? FromDateTimeStart { get; set; }
        public DateTime? UntilDateTimeEnd { get; set; }
    }
}
