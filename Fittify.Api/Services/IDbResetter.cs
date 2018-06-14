using System.Threading.Tasks;

namespace Fittify.Api.Services
{
    public interface IDbResetter
    {
        Task<bool> ResetDb();
    }
}