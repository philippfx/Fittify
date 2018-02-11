using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Patch
{
    public class DateTimeStartEndOfmForPatch : DateTimeStartEndOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }
    }
}
