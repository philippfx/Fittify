using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Fittify.Api.OuterFacingModels.Helpers;
using Fittify.Common.Helpers;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class CategoryOfmBase
    {
        [Required]
        [MaxStringLength(128)]
        public virtual string Name { get; set; }
    }
}
