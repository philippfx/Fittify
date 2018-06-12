using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fittify.Client.ApiModelRepository.Helpers;
using Fittify.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ApiModelRepository
{
    public abstract class ApiModelRepositoryBase<TId, TOfmForGet, TOfmForPost, TGetCollectionResourceParameters> : IApiModelRepository<TId, TOfmForGet, TOfmForPost, TGetCollectionResourceParameters> where TId : struct
        where TOfmForGet : class
        where TGetCollectionResourceParameters : class, new()
        where TOfmForPost : class

    {
        protected readonly IConfiguration AppConfiguration;
        protected readonly string MappedControllerActionKey;
        protected IHttpContextAccessor HttpContextAccessor;
        protected readonly IHttpRequestExecuter HttpRequestExecuter;

        public ApiModelRepositoryBase(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor, string mappedControllerActionKey, IHttpRequestExecuter httpRequestExecuter)
        {
            AppConfiguration = appConfiguration;
            MappedControllerActionKey = mappedControllerActionKey;
            HttpContextAccessor = httpContextAccessor;
            HttpRequestExecuter = httpRequestExecuter;
        }

        public virtual async Task<OfmQueryResult<TOfmForGet>> GetSingle(TId id)
        {
            var ofmQueryResult = new OfmQueryResult<TOfmForGet>();
            var uri = new Uri(
                AppConfiguration.GetValue<string>("FittifyApiBaseUrl")
                + AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
                + "/" + id
                );

            var httpResponse = await HttpRequestExecuter.GetSingle(uri, AppConfiguration, HttpContextAccessor);
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
            var httpResponse = await HttpRequestExecuter.GetSingle(uri, AppConfiguration, HttpContextAccessor);
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

            var stringUri = AppConfiguration.GetValue<string>("FittifyApiBaseUrl") +
                            AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
                            + resourceParameters.ToQueryParameterString();

            var uri = new Uri(
                stringUri
            );

            var httpResponse = await HttpRequestExecuter.GetCollection(uri, AppConfiguration, HttpContextAccessor);
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
            var httpResponse = await HttpRequestExecuter.Post(uri, ofmForPost, AppConfiguration, HttpContextAccessor);
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
            var httpResponse = await HttpRequestExecuter.Delete(uri, AppConfiguration, HttpContextAccessor);
            //var contentString = httpResponse.Content.ReadAsStringAsync();
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
            var httpResponse = await HttpRequestExecuter.Patch(uri, jsonPatchDocument, AppConfiguration, HttpContextAccessor);
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
