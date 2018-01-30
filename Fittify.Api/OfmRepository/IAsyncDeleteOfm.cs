using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncDeleteOfm<TId> where TId : struct
    {
        Task Delete(TId id);
    }
}
