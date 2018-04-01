using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.ApiModelRepositories
{
    public interface IAsyncGppd<TId, TSent, TReceived> : 
        IAsyncGet<TReceived>,
        IAsyncPost<TSent, TReceived>,
        IAsyncPatch<TReceived>,
        IAsyncDelete<TId>
        
        where TId : struct
        where TSent : class
        where TReceived : class
    {
    }
}
