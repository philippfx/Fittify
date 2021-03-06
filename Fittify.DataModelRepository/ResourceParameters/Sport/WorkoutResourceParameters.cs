﻿using System;
using Fittify.Common;
using Fittify.Common.ResourceParameters;

namespace Fittify.DataModelRepository.ResourceParameters.Sport
{
    public class WorkoutResourceParameters : EntityResourceParametersBase, ISearchQueryResourceParameters, IEntityOwner
    {
        public string SearchQuery { get; set; }
        public Guid? OwnerGuid { get; set; }
    }
}
