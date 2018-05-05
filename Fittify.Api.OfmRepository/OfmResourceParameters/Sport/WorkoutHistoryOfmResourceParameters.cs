﻿using System;
using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport
{
    public class WorkoutHistoryOfmResourceParameters : OfmResourceParametersBase, IDateTimeStartEndResourceParameters
    {
        public DateTime? FromDateTimeStart { get; set; }
        public DateTime? UntilDateTimeEnd { get; set; }
        public int? WorkoutId { get; set; }
    }
}
