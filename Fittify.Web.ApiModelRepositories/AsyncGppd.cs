using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fittify.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.ApiModelRepositories
{
    public static class AsyncGppd
    {
        public static async Task<OfmCollectionQueryResult<TOfmForGet>> GetCollection<TOfmForGet>(Uri requestBaseUri, IHttpContextAccessor httpContextAccessor) where TOfmForGet : class
        {
            var ofmQueryResult = new OfmCollectionQueryResult<TOfmForGet>();
            try
            {
                var httpResponse = await HttpRequestFactory.GetCollection(requestBaseUri);
                ofmQueryResult.HttpStatusCode = httpResponse.StatusCode;
                ofmQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

                if (!Regex.Match(((int)ofmQueryResult.HttpStatusCode).ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
                {
                    ofmQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string, object>>();
                }
                else
                {
                    ofmQueryResult.OfmForGetCollection = httpResponse.ContentAsType<IEnumerable<TOfmForGet>>();
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return ofmQueryResult;
        }

        public static async Task<OfmQueryResult<TOfmForGet>> GetSingle<TOfmForGet>(Uri requestBaseUri) where TOfmForGet : class
        {
            var ofmQueryResult = new OfmQueryResult<TOfmForGet>();
            try
            {
                var httpResponse = await HttpRequestFactory.GetSingle(requestBaseUri);
                ofmQueryResult.HttpStatusCode = httpResponse.StatusCode;
                ofmQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

                if (!Regex.Match(ofmQueryResult.HttpStatusCode.ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
                {
                    ofmQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string,object>>();
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

        public static async Task<TOfmForGet> Post<TOfmForPost, TOfmForGet>(Uri requestBaseUri, TOfmForPost ofmForPost) where TOfmForPost : class where TOfmForGet : class
        {
            var httpResponse = await HttpRequestFactory.Post(requestBaseUri, ofmForPost);
            var outputModel = httpResponse.ContentAsType<TOfmForGet>();
            return outputModel;
        }

        public static async Task<TOfmForGet> Patch<TOfmForGet>(Uri requestBaseUri, JsonPatchDocument /*object */jsonPatchDocument)
        {
            var httpResponse = await HttpRequestFactory.Patch(requestBaseUri, jsonPatchDocument);
            var outputModel = httpResponse.ContentAsType<TOfmForGet>();
            return outputModel;
        }

        public static async Task<IActionResult> Delete(Uri requestBaseUri, Controller controller)
        {
            var httpResponse = await HttpRequestFactory.Delete(requestBaseUri);
            if ((int) httpResponse.StatusCode == 204)
            {
                return controller.Ok();
            }
            else
            {
                return controller.StatusCode(500);
            }
        }
    }
}
