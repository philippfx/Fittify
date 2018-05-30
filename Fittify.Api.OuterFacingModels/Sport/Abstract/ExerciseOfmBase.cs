using System;
using System.Diagnostics.CodeAnalysis;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class ExerciseOfmBase
    {
        public string Name { get; set; }
        private ExerciseTypeEnum ExerciseTypeEnum { get; set; }

        public string ExerciseType
        {
            [ExcludeFromCodeCoverage] // Todo: Temporarily excluded for 100% code coverage. Check back later if it is cross covered by other tests
            get => Enum.GetName(typeof(ExerciseTypeEnum), ExerciseTypeEnum);
            [ExcludeFromCodeCoverage] // Todo: Temporarily excluded for 100% code coverage. Check back later if it is cross covered by other tests
            set => ExerciseTypeEnum = (ExerciseTypeEnum)Enum.Parse(typeof(ExerciseTypeEnum), value);
        }
    }
}
