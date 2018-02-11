using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Common.Helpers.ResourceParameters
{
    public interface IDateTimeStartEndResourceParameters : IResourceParameters
    {
        DateTime DateTimeStart { get; set; }
        DateTime DateTimeEnd { get; set; }
    }
}
