﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fittify.Client.ApiModelRepository.ApiModelRepository.Sport
{
    public class WorkoutHistoryApiModelRepository 
        : ApiModelRepositoryBase<int, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmCollectionResourceParameters>, IWorkoutHistoryApiModelRepository
    {
        public WorkoutHistoryApiModelRepository(
            IConfiguration appConfiguration, 
            IHttpContextAccessor httpContextAccessor, 
            ////string mappedControllerActionKey, 
            IHttpRequestExecuter httpRequestExecuter,
            ILoggerFactory logger)
            : base(
                appConfiguration,
                httpContextAccessor,
                "WorkoutHistory",
                httpRequestExecuter,
                logger)
        {

        }

        public async Task<OfmQueryResult<WorkoutHistoryOfmForGet>> Post(WorkoutHistoryOfmForPost ofmForPost, bool includeExerciseHistories)
        {
            var ofmQueryResult = new OfmQueryResult<WorkoutHistoryOfmForGet>();
            var uri = new Uri(
                AppConfiguration.GetValue<string>("FittifyApiBaseUrl") +
                AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
            );

            if (includeExerciseHistories)
            {
                uri = new Uri(uri, "?" + "includeExerciseHistories=1");
            }

            var httpResponse = await HttpRequestExecuter.Post(uri, ofmForPost, AppConfiguration, HttpContextAccessor);
            ofmQueryResult.HttpStatusCode = httpResponse.StatusCode;
            ofmQueryResult.HttpResponseHeaders = httpResponse.Headers.ToList();

            if (!Regex.Match(((int)ofmQueryResult.HttpStatusCode).ToString(), FittifyRegularExpressions.HttpStatusCodeStartsWith2).Success)
            {
                ofmQueryResult.ErrorMessagesPresented = httpResponse.ContentAsType<IReadOnlyDictionary<string, object>>();
            }
            else
            {
                ofmQueryResult.OfmForGet = httpResponse.ContentAsType<WorkoutHistoryOfmForGet>();
            }
            return ofmQueryResult;
        }
    }
}
