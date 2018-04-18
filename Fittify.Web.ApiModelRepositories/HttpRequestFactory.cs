using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Fittify.Web.ApiModelRepositories
{
    public static class HttpRequestFactory
    {
        public static async Task<HttpResponseMessage> GetSingle(Uri requestUri)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> GetSingle(Uri requestUri, Dictionary<string, string> customRequestHeaders)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri)
                .AddCustomRequestHeaders(customRequestHeaders);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> GetCollection(Uri requestUri, IHttpContextAccessor httpContextAccessor)
        {
            string accessToken = string.Empty;

            var currentContext = httpContextAccessor.HttpContext;

            accessToken = await AuthenticationHttpContextExtensions.GetTokenAsync(currentContext, OpenIdConnectParameterNames.AccessToken);

            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri)
                .AddBearerToken(accessToken);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> GetCollection(Uri requestUri, Dictionary<string, string> customRequestHeaders)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri)
                .AddCustomRequestHeaders(customRequestHeaders);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Post(
            Uri requestUri, object value)
        {
            var content = new JsonContent(value).ReadAsStringAsync().Result;
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Post)
                .AddRequestUri(requestUri)
                .AddContent(new JsonContent(value));
            
            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Put(
            Uri requestUri, object value)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Put)
                .AddRequestUri(requestUri)
                .AddContent(new JsonContent(value));

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Patch(
            Uri requestUri, JsonPatchDocument jsonPatchDocument /*object jsonPatchDocument*/)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(new HttpMethod("PATCH"))
                .AddRequestUri(requestUri)
                .AddContent(new PatchContent(jsonPatchDocument));

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Delete(Uri requestUri)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Delete)
                .AddRequestUri(requestUri);

            return await builder.SendAsync();
        }
    }
}
