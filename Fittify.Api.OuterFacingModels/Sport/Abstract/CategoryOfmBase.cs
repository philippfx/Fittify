using System.ComponentModel.DataAnnotations;
using Fittify.Api.OuterFacingModels.Helpers;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class CategoryOfmBase
    {
        [Required]
        [MaxStringLength(128)]
        public virtual string Name { get; set; }
    }
}
