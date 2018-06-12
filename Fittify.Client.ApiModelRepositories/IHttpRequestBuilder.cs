using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fittify.Client.ApiModelRepository
{
    public interface IHttpRequestBuilder
    {
        HttpRequestBuilder AddMethod(HttpMethod method);
        HttpRequestBuilder AddRequestUri(Uri requestUri);
        HttpRequestBuilder AddContent(HttpContent content);
        HttpRequestBuilder AddBearerToken(string bearerToken);
        HttpRequestBuilder AddAcceptHeader(string acceptHeader);
        HttpRequestBuilder AddTimeout(TimeSpan timeout);
        HttpRequestBuilder AddCustomRequestHeaders(Dictionary<string, string> customRequestHeaders);
        Task<HttpResponseMessage> SendAsync(HttpClient httpClient);
    }
}