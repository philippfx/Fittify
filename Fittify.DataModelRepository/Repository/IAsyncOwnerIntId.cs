using System;
using System.Threading.Tasks;

namespace Fittify.DataModelRepository.Repository
{
    public interface IAsyncEntityOwnerIntId
    {
        Task<bool> IsEntityOwner(int id, Guid ownerGuid);
    }
}
