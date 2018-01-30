using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Patch
{
    public class WorkoutOfmForPatch : IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }
    }
}
