using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ApiModelRepositories.OfmRepository.Sport
{
    public class AsyncOfmWorkoutRepository
    {
        private IConfiguration _appConfiguration;
        private IHttpContextAccessor _httpContextAccessor;

        public AsyncOfmWorkoutRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _appConfiguration = appConfiguration;
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual async Task<OfmQueryResult<WorkoutOfmForGet>> GetSingle(int id)
        {
            var ofmQueryResult = new OfmQueryResult<WorkoutOfmForGet>();
            var uri = new Uri(
                _appConfiguration.GetValue<string>("FittifyApiBaseUrl") 
                + _appConfiguration.GetValue<string>("MappedFittifyApiActions:Workout")
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
                    ofmQueryResult.OfmForGet = httpResponse.ContentAsType<WorkoutOfmForGet>();
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return ofmQueryResult;
        }

        public virtual async Task<OfmCollectionQueryResult<WorkoutOfmForGet>> GetCollection(WorkoutResourceParameters resourceParameters)
        {
            var ofmCollectionQueryResult = new OfmCollectionQueryResult<WorkoutOfmForGet>();

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
                _appConfiguration.GetValue<string>("MappedFittifyApiActions:Workout")
            );

            //var httpResponse = await HttpRequestFactory.GetCollection(new Uri(_fittifyApiBaseUri + "api/workouts" + queryParamter));
            var httpResponse = await HttpRequestFactory.GetCollection(new Uri(uri + queryParamter), _httpContextAccessor);
            ofmCollectionQueryResult.HttpStatusCode = httpResponse.StatusCode;
            ofmCollectionQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

            if (!Regex.Match(((int)ofmCollectionQueryResult.HttpStatusCode).ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
            {
                ofmCollectionQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string, object>>();
            }
            else
            {
                ofmCollectionQueryResult.OfmForGetCollection = httpResponse.ContentAsType<IEnumerable<WorkoutOfmForGet>>();
            }

            return ofmCollectionQueryResult;
        }

        public virtual async Task<OfmQueryResult<WorkoutOfmForGet>> Post(WorkoutOfmForPost workoutOfmForPost)
        {
            var ofmQueryResult = new OfmQueryResult<WorkoutOfmForGet>();
            var uri = new Uri(
                _appConfiguration.GetValue<string>("FittifyApiBaseUrl") +
                _appConfiguration.GetValue<string>("MappedFittifyApiActions:Workout")
            );
            try
            {
                var httpResponse = await HttpRequestFactory.Post(uri, workoutOfmForPost);
                ofmQueryResult.HttpStatusCode = httpResponse.StatusCode;
                ofmQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

                if (!Regex.Match(((int)ofmQueryResult.HttpStatusCode).ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
                {
                    ofmQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string, object>>();
                }
                else
                {
                    ofmQueryResult.OfmForGet = httpResponse.ContentAsType<WorkoutOfmForGet>();
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return ofmQueryResult;
        }

        public virtual async Task<OfmQueryResult<WorkoutOfmForGet>> Delete(int id)
        {
            var ofmQueryResult = new OfmQueryResult<WorkoutOfmForGet>();
            var uri = new Uri(
                _appConfiguration.GetValue<string>("FittifyApiBaseUrl")
                + _appConfiguration.GetValue<string>("MappedFittifyApiActions:Workout")
                + "/" + id
            );
            try
            {
                var httpResponse = await HttpRequestFactory.Delete(uri);
                ofmQueryResult.HttpStatusCode = httpResponse.StatusCode;
                ofmQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

                if (!Regex.Match(((int)ofmQueryResult.HttpStatusCode).ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
                {
                    ofmQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string, object>>();
                }
                else
                {
                    ofmQueryResult.OfmForGet = httpResponse.ContentAsType<WorkoutOfmForGet>();
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return ofmQueryResult;
        }

        public virtual async Task<OfmQueryResult<WorkoutOfmForGet>> Patch(int id, JsonPatchDocument jsonPatchDocument)
        {
            var ofmQueryResult = new OfmQueryResult<WorkoutOfmForGet>();
            var uri = new Uri(
                _appConfiguration.GetValue<string>("FittifyApiBaseUrl") +
                _appConfiguration.GetValue<string>("MappedFittifyApiActions:Workout")
            );
            try
            {
                var httpResponse = await HttpRequestFactory.Patch(uri, jsonPatchDocument);
                ofmQueryResult.HttpStatusCode = httpResponse.StatusCode;
                ofmQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

                if (!Regex.Match(((int)ofmQueryResult.HttpStatusCode).ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
                {
                    ofmQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string, object>>();
                }
                else
                {
                    ofmQueryResult.OfmForGet = httpResponse.ContentAsType<WorkoutOfmForGet>();
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return ofmQueryResult;
        }
    }
}
