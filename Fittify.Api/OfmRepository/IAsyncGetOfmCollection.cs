using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common.Helpers.ResourceParameters;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGetOfmCollection<TOfmForGet>
        where TOfmForGet : class
    {
        Task<IEnumerable<TOfmForGet>> GetCollection(IResourceParameters resourceParameters);
    }
}
