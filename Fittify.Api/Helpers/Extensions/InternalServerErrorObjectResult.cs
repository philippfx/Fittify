using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Fittify.Api.Helpers.Extensions
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        [JsonConstructor]
        [Obsolete("Don't use this constructor. It is required for unit testing only")]
        public InternalServerErrorObjectResult() // Todo: I need this for unit tests when deserializing expected json string to this InternalServerErrorObjectResult. Find a way to deserialize with parameters!
            : base(new SerializableError(new ModelStateDictionary()))
        {
            //StatusCode = 500;
        }

        public InternalServerErrorObjectResult(ModelStateDictionary modelState)
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }
            StatusCode = 500;
        }
    }
}