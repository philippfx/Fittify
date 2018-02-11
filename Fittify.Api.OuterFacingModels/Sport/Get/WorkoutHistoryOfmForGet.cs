using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Helpers;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class WorkoutHistoryOfmForGet : WorkoutHistoryOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }
    }
}