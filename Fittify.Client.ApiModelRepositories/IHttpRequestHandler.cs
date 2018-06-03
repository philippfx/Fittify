using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ApiModelRepositories
{
    public interface IHttpRequestHandler
    {
        Task<HttpResponseMessage> GetSingle(Uri requestUri, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor);
        Task<HttpResponseMessage> GetCollection(Uri requestUri, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor);

        Task<HttpResponseMessage> Post(
            Uri requestUri, object value, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor);

        Task<HttpResponseMessage> Put(
            Uri requestUri, object value);

        Task<HttpResponseMessage> Patch(
            Uri requestUri, JsonPatchDocument jsonPatchDocument /*object jsonPatchDocument*/, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor);

        Task<HttpResponseMessage> Delete(Uri requestUri, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor);
    }
}