using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository.Unowned
{
    public interface IAsyncPostOfm<TOfmForGet, in TOfmForPost> where TOfmForPost : class
    {
        Task<TOfmForGet> Post(TOfmForPost entity);
    }
}
