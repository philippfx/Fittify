using System;
using System.Threading.Tasks;

namespace Fittify.DataModelRepositories.Repository
{
    public interface IAsyncOwnerIntId
    {
        Task<bool> IsEntityOwner(int id, Guid ownerGuid);
    }
}
