using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fittify.Client.ApiModelRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ViewModelRepository.Test.TestHelpers
{
    /// <summary>
    /// Ignores Access Token handling. These access tokens are not necessary, because an authenticated user mock is injected via headers into each request for the ApiTestServer. Otherwise, the IDP Server needs to be mocked as well.
    /// </summary>
    public class HttpRequestExecuterForIntegrationTest : IHttpRequestExecuter
    {
        private readonly IHttpRequestBuilder _httpRequestBuilder;
        public HttpRequestExecuterForIntegrationTest(IHttpRequestBuilder httpRequestBuilder)
        {
            _httpRequestBuilder = httpRequestBuilder;
        }
        public async Task<HttpResponseMessage> GetSingle(Uri requestUri, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _httpRequestBuilder
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri);

            return await _httpRequestBuilder.SendAsync();
        }

        public async Task<HttpResponseMessage> GetCollection(Uri requestUri, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _httpRequestBuilder
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri);


            var builder = _httpRequestBuilder
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri);

            return await _httpRequestBuilder.SendAsync();
        }

        public async Task<HttpResponseMessage> Post(
            Uri requestUri, object value, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _httpRequestBuilder
                .AddMethod(HttpMethod.Post)
                .AddRequestUri(requestUri)
                .AddContent(new JsonContent(value));

            return await _httpRequestBuilder.SendAsync();
        }

        public async Task<HttpResponseMessage> Put(
            Uri requestUri, object value)
        {
            _httpRequestBuilder
                .AddMethod(HttpMethod.Put)
                .AddRequestUri(requestUri)
                .AddContent(new JsonContent(value));

            return await _httpRequestBuilder.SendAsync();
        }

        public async Task<HttpResponseMessage> Patch(
            Uri requestUri, JsonPatchDocument jsonPatchDocument /*object jsonPatchDocument*/, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _httpRequestBuilder
                .AddMethod(new HttpMethod("PATCH"))
                .AddRequestUri(requestUri)
                .AddContent(new PatchContent(jsonPatchDocument));

            return await _httpRequestBuilder.SendAsync();
        }

        public async Task<HttpResponseMessage> Delete(Uri requestUri, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _httpRequestBuilder
                .AddMethod(HttpMethod.Delete)
                .AddRequestUri(requestUri);

            return await _httpRequestBuilder.SendAsync();
        }
    }
}

