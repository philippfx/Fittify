using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace Fittify.Web.ApiModelRepositories
{
    public class HttpRequestFactory
    {
        public static async Task<HttpResponseMessage> GetSingle(string requestUri)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> GetSingle(string requestUri, Dictionary<string, string> customRequestHeaders)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri)
                .AddCustomRequestHeaders(customRequestHeaders);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> GetCollection(string requestUri)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> GetCollection(string requestUri, Dictionary<string, string> customRequestHeaders)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri)
                .AddCustomRequestHeaders(customRequestHeaders);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Post(
            string requestUri, object value)
        {
            var content = new JsonContent(value).ReadAsStringAsync().Result;
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Post)
                .AddRequestUri(requestUri)
                .AddContent(new JsonContent(value));
            
            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Put(
            string requestUri, object value)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Put)
                .AddRequestUri(requestUri)
                .AddContent(new JsonContent(value));

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Patch(
            string requestUri, JsonPatchDocument jsonPatchDocument /*object jsonPatchDocument*/)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(new HttpMethod("PATCH"))
                .AddRequestUri(requestUri)
                .AddContent(new PatchContent(jsonPatchDocument));

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Delete(string requestUri)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Delete)
                .AddRequestUri(requestUri);

            return await builder.SendAsync();
        }
    }
}
