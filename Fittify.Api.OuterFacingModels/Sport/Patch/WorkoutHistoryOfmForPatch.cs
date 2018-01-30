using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Patch
{
    public class WorkoutHistoryOfmForPatch : WorkoutHistoryOfmBase, IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }
    }
}