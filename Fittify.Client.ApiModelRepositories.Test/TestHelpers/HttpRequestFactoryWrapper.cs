////using System;
////using System.Collections.Generic;
////using System.Globalization;
////using System.Net.Http;
////using System.Text;
////using System.Threading.Tasks;
////using Fittify.Common.CustomExceptions;
////using Fittify.Web.ApiModelRepositories;
////using IdentityModel.Client;
////using Microsoft.AspNetCore.Authentication;
////using Microsoft.AspNetCore.Http;
////using Microsoft.AspNetCore.JsonPatch;
////using Microsoft.Extensions.Configuration;
////using Microsoft.IdentityModel.Protocols.OpenIdConnect;

////namespace Fittify.Client.ApiModelRepositories.Test.TestHelpers
////{
////    public class HttpRequestFactoryWrapper
////    {
////        public async Task<HttpResponseMessage> GetSingle(Uri requestUri, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
////        {
////            return await HttpRequestHandler.GetSingle(requestUri, appConfiguration, httpContextAccessor);
////        }

////        public async Task<HttpResponseMessage> GetCollection(Uri requestUri, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
////        {
////            return await HttpRequestHandler.GetCollection(requestUri, appConfiguration, httpContextAccessor);
////        }

////        public async Task<HttpResponseMessage> Post(
////            Uri requestUri, object value, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
////        {
////            return await HttpRequestHandler.Post(requestUri, value, appConfiguration, httpContextAccessor);
////        }

////        public async Task<HttpResponseMessage> Put(
////            Uri requestUri, object value)
////        {
////            return await HttpRequestHandler.Put(requestUri, value);
////        }

////        public async Task<HttpResponseMessage> Patch(
////            Uri requestUri, JsonPatchDocument jsonPatchDocument, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
////        {
////            return await HttpRequestHandler.Patch(requestUri, jsonPatchDocument, appConfiguration, httpContextAccessor);
////        }

////        public async Task<HttpResponseMessage> Delete(Uri requestUri, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
////        {
////            return await HttpRequestHandler.Delete(requestUri, appConfiguration, httpContextAccessor);
////        }
////    }
////}
