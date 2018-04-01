using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.ApiModelRepositories
{
    public static class AsyncGppd
    {
        public static async Task<IEnumerable<TOfmForGet>> GetCollection<TOfmForGet>(string requestBaseUri) where TOfmForGet : class
        {
            IEnumerable<TOfmForGet> outputModel = null;
            try
            {
                var httpResponse = await HttpRequestFactory.GetCollection(requestBaseUri);
                if ((int)httpResponse.StatusCode != 404)
                {
                    outputModel = httpResponse.ContentAsType<IEnumerable<TOfmForGet>>();
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return outputModel;
        }

        public static async Task<TOfmForGet> GetSingle<TOfmForGet>(string requestBaseUri) where TOfmForGet : class
        {
            TOfmForGet outputModel = null;
            try
            {
                var httpResponse = await HttpRequestFactory.GetSingle(requestBaseUri);
                outputModel = httpResponse.ContentAsType<TOfmForGet>();
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return outputModel;
        }

        public static async Task<TOfmForGet> Post<TOfmForPost, TOfmForGet>(string requestBaseUri, TOfmForPost ofmForPost) where TOfmForPost : class where TOfmForGet : class
        {
            var httpResponse = await HttpRequestFactory.Post(requestBaseUri, ofmForPost);
            var outputModel = httpResponse.ContentAsType<TOfmForGet>();
            return outputModel;
        }

        public static async Task<TOfmForGet> Patch<TOfmForGet>(string requestBaseUri, JsonPatchDocument jsonPatchDocument)
        {
            var httpResponse = await HttpRequestFactory.Patch(requestBaseUri, jsonPatchDocument);
            var outputModel = httpResponse.ContentAsType<TOfmForGet>();
            return outputModel;
        }

        public static async Task<IActionResult> Delete(string requestBaseUri, Controller controller)
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
