﻿using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport
{
    public class WorkoutOfmResourceParameters : OfmResourceParametersBase, ISearchQueryResourceParameters
    {
        public string SearchQuery { get; set; }
        public int? CategoryId { get; set; }
    }
}
