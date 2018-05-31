using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository.OfmRepository
{
    public interface IAsyncOfmOwnerIntId
    {
        Task<bool> IsEntityOwner(int id, Guid ownerGuid);
    }
}
