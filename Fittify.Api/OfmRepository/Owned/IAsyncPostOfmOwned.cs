using System;
using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository.Owned
{
    public interface IAsyncPostOfmOwned<TOfmForGet, in TOfmForPost> where TOfmForPost : class
    {
        Task<TOfmForGet> Post(TOfmForPost entity, Guid ownerGuid);
    }
}
