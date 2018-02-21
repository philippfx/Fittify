using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository
{
    interface IAsyncGetOfmByNameSearch<TOfmForGet, TId> : IAsyncGetOfm<TOfmForGet, TId>
        where TOfmForGet : class
        where TId : struct
    {

    }
}
