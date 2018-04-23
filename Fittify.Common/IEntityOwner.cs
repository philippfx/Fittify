using System;

namespace Fittify.Common
{
    public interface IEntityOwner
    {
        Guid? OwnerGuid { get; set; }
    }
}
