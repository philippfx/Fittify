namespace Fittify.Web.ApiModelRepositories
{
    public interface IAsyncGppd<TId, TSent, TReceived> : 
        IAsyncGet<TId, TReceived>,
        IAsyncPost<TSent, TReceived>,
        IAsyncPatch<TReceived>,
        IAsyncDelete<TId>
        
        where TId : struct
        where TSent : class
        where TReceived : class
    {
    }
}
