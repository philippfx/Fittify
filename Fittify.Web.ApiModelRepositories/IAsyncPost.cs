using System.Threading.Tasks;

namespace Fittify.Web.ApiModelRepositories
{
    public interface IAsyncPost<TSent, TReceived>
        where TSent : class
        where TReceived : class
    {
        Task<TReceived> Post(TSent entity);
    }
}
