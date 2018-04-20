using System;
using System.Threading.Tasks;
using Fittify.Api.Helpers;

namespace Fittify.Api.OfmRepository.Owned
{
    public interface IAsyncGetOfmByIdOwned<TOfmForGet, TId>
        where TOfmForGet : class
        where TId : struct
    {
        Task<OfmForGetQueryResult<TOfmForGet>> GetById(TId id, string fields, Guid ownerGuid);
    }
}