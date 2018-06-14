using System.Threading.Tasks;

namespace Quantus.IDP.Services
{
    public interface IDbResetter
    {
        Task<bool> ResetDb();
    }
}