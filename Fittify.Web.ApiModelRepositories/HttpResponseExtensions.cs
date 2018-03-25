﻿using System.Net.Http;
using Newtonsoft.Json;

namespace Fittify.Web.ApiModelRepositories
{
    public static class HttpResponseExtensions
    {
        public static T ContentAsType<T>(this HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;

            // Todo dirty workaround
            //if (data[0].ToString() != "[")
            //{
            //    data = "[" + data + "]";
            //}
            //try
            //{
                return string.IsNullOrEmpty(data) ?
                    default(T) :
                    JsonConvert.DeserializeObject<T>(data);
            //}
            //catch (AggregateException e)
            //{
            //    return string.IsNullOrEmpty(data) ?
            //        default(T) :
            //        JsonConvert.DeserializeObject<T>(data);
            //}
        }

        public static string ContentAsJson(this HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.SerializeObject(data);
        }

        public static string ContentAsString(this HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
