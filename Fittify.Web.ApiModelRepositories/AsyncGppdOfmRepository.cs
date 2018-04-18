using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.ApiModelRepositories
{

    public class AsyncGppdOfmRepository<TId, TSent, TReceived> : IAsyncGppd<TId, TSent, TReceived>
        where TId : struct
        where TSent : class
        where TReceived : class
    {
        protected Uri RequestBaseUri;
        protected HttpResponseMessage HttpResponse;
        public AsyncGppdOfmRepository(Uri requestBaseUri)
        {
            RequestBaseUri = requestBaseUri;
        }

        public AsyncGppdOfmRepository()
        {
            
        }

        public virtual async Task<TReceived> GetSingle(TId id)
        {
            TReceived outputModel = null;
            try
            {
                HttpResponse = await HttpRequestFactory.GetSingle(RequestBaseUri);
                outputModel = HttpResponse.ContentAsType<TReceived>();
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return outputModel;
        }

        public virtual async Task<IEnumerable<TReceived>> GetCollection(IHttpContextAccessor httpContextAccessor)
        {
            IEnumerable<TReceived> outputModel = null;
            try
            {
                HttpResponse = await HttpRequestFactory.GetCollection(RequestBaseUri, httpContextAccessor);
                outputModel = HttpResponse.ContentAsType<IEnumerable<TReceived>>();
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return outputModel;
        }

        public virtual async Task<TReceived> Post(TSent entity)
        {
            HttpResponse = await HttpRequestFactory.Post(RequestBaseUri, entity);
            var resultString = HttpResponse.ContentAsString();
            var outputModel = HttpResponse.ContentAsType<TReceived>();
            return outputModel;
        }
        
        public virtual async Task<TReceived> Patch(JsonPatchDocument jsonPatchDocument)
        {
            HttpResponse = await HttpRequestFactory.Patch(RequestBaseUri, jsonPatchDocument);
            var outputModel = HttpResponse.ContentAsType<TReceived>();
            return outputModel;
        }

        public virtual async Task<IActionResult> Delete(TId id)
        {
            HttpResponse = await HttpRequestFactory.Delete(RequestBaseUri);
            return new JsonResult("not implemented");
        }
    }
}
