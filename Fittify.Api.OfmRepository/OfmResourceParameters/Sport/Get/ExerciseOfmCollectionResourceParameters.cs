﻿using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get
{
    public class ExerciseOfmCollectionResourceParameters : OfmResourceParametersBase, ISearchQueryResourceParameters
    {
        public string SearchQuery { get; set; }
    }
}
