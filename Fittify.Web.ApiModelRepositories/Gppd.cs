using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.ApiModelRepositories
{

    public class GppdRepository<T> : Controller, IAsyncGppd<T> where T : class
    {
        protected readonly string RequestUri;
        protected HttpResponseMessage HttpResponse;
        public GppdRepository(string requestUri)
        {
            RequestUri = requestUri;
        }


        public virtual async Task<IActionResult> Delete()
        {
            HttpResponse = await HttpRequestFactory.Delete(RequestUri);
            return Ok();
        }

        public virtual async Task<IEnumerable<T>> Get()
        {
            IEnumerable<T> outputModel = null;
            try
            {
                HttpResponse = await HttpRequestFactory.Get(RequestUri);
                outputModel = HttpResponse.ContentAsType<IEnumerable<T>>();
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return outputModel;
        }

        public virtual async Task<IEnumerable<T>> GetByRangeOfIds(string rangeOfIds)
        {
            HttpResponse = await HttpRequestFactory.Get(RequestUri + "/" + rangeOfIds);
            var outputModel = HttpResponse.ContentAsType<IEnumerable<T>>();
            return outputModel;
        }

        public virtual async Task<T> Post(T entity)
        {
            HttpResponse = await HttpRequestFactory.Post(RequestUri, entity);
            var resultString = HttpResponse.ContentAsString();
            var outputModel = HttpResponse.ContentAsType<T>();
            return outputModel;
        }

        public virtual Task<IActionResult> Put(T entity)
        {
            throw new System.NotImplementedException();
        }
        public virtual async Task<T> Patch(JsonPatchDocument jsonPatchDocument)
        {
            HttpResponse = await HttpRequestFactory.Patch(RequestUri, jsonPatchDocument);
            var outputModel = HttpResponse.ContentAsType<T>();
            return outputModel;
        }
    }
}
