using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Common.Helpers.ResourceParameters
{
    public class DateTimeStartEndResourceParameters : ResourceParameters, IDateTimeStartEndResourceParameters
    {
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
    }
}
