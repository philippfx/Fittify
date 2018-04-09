using System;
using System.Collections.Generic;
using System.Text;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public class ExerciseOfmBase
    {
        public string Name { get; set; }
        private ExerciseTypeEnum ExerciseTypeEnum { get; set; }

        public string ExerciseType
        {
            get => Enum.GetName(typeof(ExerciseTypeEnum), ExerciseTypeEnum);
            set => ExerciseTypeEnum = (ExerciseTypeEnum)Enum.Parse(typeof(ExerciseTypeEnum), value);
        }
    }
}
