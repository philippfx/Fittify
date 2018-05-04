using System;
using System.Threading.Tasks;

namespace Fittify.DataModelRepository.Repository
{
    public interface IAsyncOwnerIntId
    {
        Task<bool> IsEntityOwner(int id, Guid ownerGuid);
    }
}
