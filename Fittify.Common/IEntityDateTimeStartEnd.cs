using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Common
{
    public interface IEntityDateTimeStartEnd<TId> : IEntityUniqueIdentifier<TId> where TId: struct
    {
        DateTime? DateTimeStart { get; set; }
        DateTime? DateTimeEnd { get; set; }
    }
}
