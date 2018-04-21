using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Common
{
    public interface IEntityOwner
    {
        Guid? OwnerGuid { get; set; }
    }
}
