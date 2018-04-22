using System;

namespace Fittify.DataModelRepositories.Owned
{
    public interface IAsyncOwnerIntId
    {
        bool IsEntityOwner(int id, Guid ownerGuid);
    }
}
