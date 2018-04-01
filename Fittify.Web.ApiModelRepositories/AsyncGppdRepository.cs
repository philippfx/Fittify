using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.ApiModelRepositories
{

    public class AsyncGppdRepository<TId, TSent, TReceived> : IAsyncGppd<TId, TSent, TReceived>
        where TId : struct
        where TSent : class
        where TReceived : class
    {
        protected string RequestBaseUri;
        protected HttpResponseMessage HttpResponse;
        public AsyncGppdRepository(string requestBaseUri)
        {
            RequestBaseUri = requestBaseUri;
        }

        public AsyncGppdRepository()
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

        public virtual async Task<IEnumerable<TReceived>> GetCollection()
        {
            IEnumerable<TReceived> outputModel = null;
            try
            {
                HttpResponse = await HttpRequestFactory.GetCollection(RequestBaseUri);
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

        public Task<TReceived> GetSingle()
        {
            throw new NotImplementedException();
        }
    }
}
