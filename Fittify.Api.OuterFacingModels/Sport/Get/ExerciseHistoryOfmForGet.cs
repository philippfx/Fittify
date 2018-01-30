using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class ExerciseHistoryOfmForGet : ExerciseHistoryOfmBase, IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }

        public class ExerciseOfm : ExerciseOfmBase, IUniqueIdentifierDataModels<int>
        {
            public int Id { get; set; }
        }
        public ExerciseOfm Exercise { get; set; }
    }

}
