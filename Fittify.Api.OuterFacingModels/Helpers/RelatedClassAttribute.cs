using System;

namespace Fittify.Api.OuterFacingModels.Helpers
{
    public class RelatedClassAttribute : Attribute
    {
        public RelatedClassAttribute(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; set; }
    }
}
