using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fittify.Web.ApiModelRepositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.ViewModelRepository.Sport
{

    public class AsyncViewModelRepository<TId, TOfmForPost, TViewModelReceived> : IAsyncGppd<TId, TOfmForPost, TViewModelReceived>
        where TId : struct
        where TOfmForPost : class
        where TViewModelReceived : class
    {
        protected Uri RequestBaseUri;
        protected HttpResponseMessage HttpResponse;
        public AsyncViewModelRepository(Uri requestBaseUri)
        {
            RequestBaseUri = requestBaseUri;
        }

        public AsyncViewModelRepository()
        {
            
        }

        public virtual async Task<TViewModelReceived> GetSingle(TId id)
        {
            TViewModelReceived outputModel = null;
            try
            {
                HttpResponse = await HttpRequestFactory.GetSingle(RequestBaseUri);
                outputModel = HttpResponse.ContentAsType<TViewModelReceived>();
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return outputModel;
        }

        public virtual async Task<IEnumerable<TViewModelReceived>> GetCollection()
        {
            IEnumerable<TViewModelReceived> outputModel = null;
            try
            {
                HttpResponse = await HttpRequestFactory.GetCollection(RequestBaseUri);
                outputModel = HttpResponse.ContentAsType<IEnumerable<TViewModelReceived>>();
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return outputModel;
        }

        public virtual async Task<TViewModelReceived> Post(TOfmForPost entity)
        {
            HttpResponse = await HttpRequestFactory.Post(RequestBaseUri, entity);
            var resultString = HttpResponse.ContentAsString();
            var outputModel = HttpResponse.ContentAsType<TViewModelReceived>();
            return outputModel;
        }
        
        public virtual async Task<TViewModelReceived> Patch(JsonPatchDocument jsonPatchDocument)
        {
            HttpResponse = await HttpRequestFactory.Patch(RequestBaseUri, jsonPatchDocument);
            var outputModel = HttpResponse.ContentAsType<TViewModelReceived>();
            return outputModel;
        }

        public virtual async Task<IActionResult> Delete(TId id)
        {
            HttpResponse = await HttpRequestFactory.Delete(RequestBaseUri);
            return new JsonResult("not implemented");
        }
    }
}
