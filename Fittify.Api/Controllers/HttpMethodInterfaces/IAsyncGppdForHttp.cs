namespace Fittify.Api.Controllers.HttpMethodInterfaces
{
    public interface IAsyncGppdForHttp<TId, in TOfmForPost, TOfmForPatch> :
        IAsyncGetForHttp<TId>,
        IAsyncPostForHttp<TOfmForPost>,
        IAsyncPatchForHttp<TOfmForPatch, TId>,
        IAsyncDeleteForHttp<TId>

        where TId : struct
        where TOfmForPost : class
        where TOfmForPatch : class
    {

    }
}
