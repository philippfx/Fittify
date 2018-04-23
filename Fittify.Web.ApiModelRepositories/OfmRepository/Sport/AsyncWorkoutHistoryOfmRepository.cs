using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ApiModelRepositories.OfmRepository.Sport
{
    public class AsyncWorkoutHistoryOfmRepository : GenericAsyncGppdOfm<int, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryResourceParameters>
    {
        public AsyncWorkoutHistoryOfmRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor, string mappedControllerActionKey)
            : base(appConfiguration, httpContextAccessor, mappedControllerActionKey)
        {

        }

        public override async Task<OfmQueryResult<WorkoutHistoryOfmForGet>> Post(WorkoutHistoryOfmForPost ofmForPost)
        {
            var ofmQueryResult = new OfmQueryResult<WorkoutHistoryOfmForGet>();
            var uri = new Uri(
                AppConfiguration.GetValue<string>("FittifyApiBaseUrl") +
                AppConfiguration.GetValue<string>("MappedFittifyApiActions:" + MappedControllerActionKey)
                + "?" + "includeExerciseHistories=1"
            );
            var httpResponse = await HttpRequestFactory.Post(uri, ofmForPost, AppConfiguration, HttpContextAccessor);
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
