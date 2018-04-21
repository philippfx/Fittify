using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fittify.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ApiModelRepositories
{
    public class GenericAsyncGppdOfm<TId, TOfmForGet, TOfmForPost, TResourceParameters>
        where TId : struct
        where TOfmForGet : class
        where TResourceParameters : class
        where TOfmForPost : class

    {
        protected readonly IConfiguration AppConfiguration;
        protected readonly string MappedControllerActionKey;
        protected IHttpContextAccessor HttpContextAccessor;

        public GenericAsyncGppdOfm(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor, string mappedControllerActionKey)
        {
            AppConfiguration = appConfiguration;
            MappedControllerActionKey = mappedControllerActionKey;
            HttpContextAccessor = httpContextAccessor;
        }

        public virtual async Task<OfmQueryResult<TOfmForGet>> GetSingle(TId id)
        {
            var ofmQueryResult = new OfmQueryResult<TOfmForGet>();
            var uri = new Uri(
                AppConfiguration.GetValue<string>("FittifyApiBaseUrl")
                + AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
                + "/" + id
                );
            try
            {
                var httpResponse = await HttpRequestFactory.GetSingle(uri, HttpContextAccessor);
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
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return ofmQueryResult;
        }

        public virtual async Task<OfmCollectionQueryResult<TOfmForGet>> GetCollection(TResourceParameters resourceParameters)
        {
            var ofmCollectionQueryResult = new OfmCollectionQueryResult<TOfmForGet>();

            PropertyInfo[] properties = resourceParameters.GetType().GetProperties();
            string queryParamter = "";
            foreach (var p in properties)
            {
                var pName = p.Name;
                var pVal = p.GetValue(resourceParameters);

                if (pVal != null)
                {
                    if (pVal as string != null && pVal as string != "")
                    {
                        if (String.IsNullOrWhiteSpace(queryParamter))
                        {
                            queryParamter = "?" + pName + "=" + pVal;
                        }
                        else
                        {
                            queryParamter += "&" + pName + "=" + pVal;
                        }
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(queryParamter))
                        {
                            queryParamter = "?" + pName + "=" + pVal;
                        }
                        else
                        {
                            queryParamter += "&" + pName + "=" + pVal;
                        }
                    }
                }
            }

            var uri = new Uri(
                AppConfiguration.GetValue<string>("FittifyApiBaseUrl") +
                AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
            );

            var httpResponse = await HttpRequestFactory.GetCollection(new Uri(uri + queryParamter), HttpContextAccessor);
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
            var httpResponse = await HttpRequestFactory.Post(uri, ofmForPost, HttpContextAccessor);
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
            var httpResponse = await HttpRequestFactory.Delete(uri, HttpContextAccessor);
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

        public virtual async Task<OfmQueryResult<TOfmForGet>> Patch(TId id, JsonPatchDocument jsonPatchDocument)
        {
            var ofmQueryResult = new OfmQueryResult<TOfmForGet>();
            var uri = new Uri(
                AppConfiguration.GetValue<string>("FittifyApiBaseUrl")
                + AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
                + "/" + id
            );
            var httpResponse = await HttpRequestFactory.Patch(uri, jsonPatchDocument, HttpContextAccessor);
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
