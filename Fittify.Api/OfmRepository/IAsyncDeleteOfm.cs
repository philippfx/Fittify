using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncDeleteOfm<TId> where TId : struct
    {
        Task<bool> Delete(TId id);
    }
}
