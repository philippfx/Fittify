using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fittify.Client.ApiModelRepositories.Helpers;
using Fittify.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ApiModelRepositories
{
    public class GenericAsyncGppdOfm<TId, TOfmForGet, TOfmForPost, TGetCollectionResourceParameters> 
        where TId : struct
        where TOfmForGet : class
        where TGetCollectionResourceParameters : class, new()
        where TOfmForPost : class

    {
        protected readonly IConfiguration AppConfiguration;
        protected readonly string MappedControllerActionKey;
        protected IHttpContextAccessor HttpContextAccessor;
        protected readonly IHttpRequestHandler HttpRequestHandler;

        public GenericAsyncGppdOfm(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor, string mappedControllerActionKey, IHttpRequestHandler httpRequestHandler)
        {
            AppConfiguration = appConfiguration;
            MappedControllerActionKey = mappedControllerActionKey;
            HttpContextAccessor = httpContextAccessor;
            HttpRequestHandler = httpRequestHandler;
        }

        public virtual async Task<OfmQueryResult<TOfmForGet>> GetSingle(TId id)
        {
            var ofmQueryResult = new OfmQueryResult<TOfmForGet>();
            var uri = new Uri(
                AppConfiguration.GetValue<string>("FittifyApiBaseUrl")
                + AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
                + "/" + id
                );

            var httpResponse = await HttpRequestHandler.GetSingle(uri, AppConfiguration, HttpContextAccessor);
            var contentAsString = httpResponse.Content.ReadAsStringAsync();
            ofmQueryResult.HttpStatusCode = httpResponse.StatusCode;
            ofmQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

            if (!Regex.Match(((int)ofmQueryResult.HttpStatusCode).ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
            {
                ofmQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string, object>>();
            }
            else
            {
                ofmQueryResult.OfmForGet = httpResponse.ContentAsType<TOfmForGet>();
            }
            return ofmQueryResult;
        }

        public virtual async Task<OfmQueryResult<TOfmForGet>> GetSingle<TGetResourceParameters>(TId id, TGetResourceParameters resourceParameters) where TGetResourceParameters : class
        {
            var ofmQueryResult = new OfmQueryResult<TOfmForGet>();
            
            var uri = new Uri(
                AppConfiguration.GetValue<string>("FittifyApiBaseUrl")
                + AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
                + "/" + id + resourceParameters.ToQueryParameterString()
            );
            var httpResponse = await HttpRequestHandler.GetSingle(uri, AppConfiguration, HttpContextAccessor);
            var contentAsString = httpResponse.Content.ReadAsStringAsync();
            ofmQueryResult.HttpStatusCode = httpResponse.StatusCode;
            ofmQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

            if (!Regex.Match(((int)ofmQueryResult.HttpStatusCode).ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
            {
                ofmQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string, object>>();
            }
            else
            {
                ofmQueryResult.OfmForGet = httpResponse.ContentAsType<TOfmForGet>();
            }
            return ofmQueryResult;
        }

        public virtual async Task<OfmCollectionQueryResult<TOfmForGet>> GetCollection(TGetCollectionResourceParameters resourceParameters)
        {
            var ofmCollectionQueryResult = new OfmCollectionQueryResult<TOfmForGet>();

            if (resourceParameters == null)
            {
                resourceParameters = new TGetCollectionResourceParameters();
            }

            var uri = new Uri(
                AppConfiguration.GetValue<string>("FittifyApiBaseUrl") +
                AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
                + resourceParameters.ToQueryParameterString()
            );

            var httpResponse = await HttpRequestHandler.GetCollection(uri, AppConfiguration, HttpContextAccessor);
            var contentAsString = httpResponse.Content.ReadAsStringAsync();
            ofmCollectionQueryResult.HttpStatusCode = httpResponse.StatusCode;
            ofmCollectionQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

            if (!Regex.Match(((int)ofmCollectionQueryResult.HttpStatusCode).ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
            {
                ofmCollectionQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string, object>>();
            }
            else
            {
                ofmCollectionQueryResult.OfmForGetCollection = httpResponse.ContentAsType<IEnumerable<TOfmForGet>>();
            }

            return ofmCollectionQueryResult;
        }

        public virtual async Task<OfmQueryResult<TOfmForGet>> Post(TOfmForPost ofmForPost)
        {
            var ofmQueryResult = new OfmQueryResult<TOfmForGet>();
            var uri = new Uri(
                AppConfiguration.GetValue<string>("FittifyApiBaseUrl") +
                AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
            );
            var httpResponse = await HttpRequestHandler.Post(uri, ofmForPost, AppConfiguration, HttpContextAccessor);
            ofmQueryResult.HttpStatusCode = httpResponse.StatusCode;
            ofmQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

            if (!Regex.Match(((int)ofmQueryResult.HttpStatusCode).ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
            {
                ofmQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string, object>>();
            }
            else
            {
                ofmQueryResult.OfmForGet = httpResponse.ContentAsType<TOfmForGet>();
            }
            return ofmQueryResult;
        }

        public virtual async Task<OfmQueryResult<TOfmForGet>> Delete(TId id)
        {
            var ofmQueryResult = new OfmQueryResult<TOfmForGet>();
            var uri = new Uri(
                AppConfiguration.GetValue<string>("FittifyApiBaseUrl")
                + AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
                + "/" + id
            );
            var httpResponse = await HttpRequestHandler.Delete(uri, AppConfiguration, HttpContextAccessor);
            ofmQueryResult.HttpStatusCode = httpResponse.StatusCode;
            ofmQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

            if (!Regex.Match(((int)ofmQueryResult.HttpStatusCode).ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
            {
                ofmQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string, object>>();
            }
            return ofmQueryResult;
        }

        public virtual async Task<OfmQueryResult<TOfmForGet>> Patch(TId id, JsonPatchDocument jsonPatchDocument)
        {
            var ofmQueryResult = new OfmQueryResult<TOfmForGet>();
            var uri = new Uri(
                AppConfiguration.GetValue<string>("FittifyApiBaseUrl")
                + AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
                + "/" + id
            );
            var httpResponse = await HttpRequestHandler.Patch(uri, jsonPatchDocument, AppConfiguration, HttpContextAccessor);
            ofmQueryResult.HttpStatusCode = httpResponse.StatusCode;
            ofmQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

            if (!Regex.Match(((int)ofmQueryResult.HttpStatusCode).ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
            {
                ofmQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string, object>>();
            }
            else
            {
                ofmQueryResult.OfmForGet = httpResponse.ContentAsType<TOfmForGet>();
            }
            return ofmQueryResult;
        }
    }
}
