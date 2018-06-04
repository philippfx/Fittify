using System;
using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository.OfmRepository
{
    public interface IAsyncOfmOwnerIntId
    {
        Task<bool> IsEntityOwner(int id, Guid ownerGuid);
    }
}
