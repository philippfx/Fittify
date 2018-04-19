using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fittify.Common.Helpers;
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
        private readonly IConfiguration _appConfiguration;
        private readonly string _mappedControllerActionKey;

        public GenericAsyncGppdOfm(IConfiguration appConfiguration, string mappedControllerActionKey)
        {
            _appConfiguration = appConfiguration;
            _mappedControllerActionKey = mappedControllerActionKey;
        }

        public virtual async Task<OfmQueryResult<TOfmForGet>> GetSingle(TId id)
        {
            var ofmQueryResult = new OfmQueryResult<TOfmForGet>();
            var uri = new Uri(
                _appConfiguration.GetValue<string>("FittifyApiBaseUrl")
                + _appConfiguration.GetValue<string>("MappedFittifyApiActions:" + _mappedControllerActionKey)
                + "/" + id
                );
            try
            {
                var httpResponse = await HttpRequestFactory.GetSingle(uri);
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
                _appConfiguration.GetValue<string>("FittifyApiBaseUrl") +
                _appConfiguration.GetValue<string>("MappedFittifyApiActions:" + _mappedControllerActionKey)
            );

            var httpResponse = await HttpRequestFactory.GetCollection(new Uri(uri + queryParamter));
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
                _appConfiguration.GetValue<string>("FittifyApiBaseUrl") +
                _appConfiguration.GetValue<string>("MappedFittifyApiActions:" + _mappedControllerActionKey)
            );
            var httpResponse = await HttpRequestFactory.Post(uri, ofmForPost);
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
                _appConfiguration.GetValue<string>("FittifyApiBaseUrl")
                + _appConfiguration.GetValue<string>("MappedFittifyApiActions:" + _mappedControllerActionKey)
                + "/" + id
            );
            var httpResponse = await HttpRequestFactory.Delete(uri);
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
                _appConfiguration.GetValue<string>("FittifyApiBaseUrl")
                + _appConfiguration.GetValue<string>("MappedFittifyApiActions:" + _mappedControllerActionKey)
                + "/" + id
            );
            var httpResponse = await HttpRequestFactory.Patch(uri, jsonPatchDocument);
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
