using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class CardioSetOfmBase
    {
        public virtual DateTime? DateTimeStart { get; set; }
        public virtual DateTime? DateTimeEnd { get; set; }
        [Required]
        public virtual int ExerciseHistoryId { get; set; }
    }
}
