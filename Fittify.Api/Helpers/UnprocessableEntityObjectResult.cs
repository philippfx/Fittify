using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Fittify.Api.Helpers
{
    public class UnprocessableEntityObjectResult : ObjectResult
    {
        [JsonConstructor]
        [Obsolete("Don't use this constructor. It is required for unit testing only")]
        public UnprocessableEntityObjectResult() // Todo: I need this for unit tests when deserializing expected json string to this UnprocessableEntityObjectResult. Find a way to deserialize with parameters!
            : base(new SerializableError(new ModelStateDictionary()))
        {
            //StatusCode = 422; // Set by deserialized object
        }

        public UnprocessableEntityObjectResult(ModelStateDictionary modelState)
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }
            StatusCode = 422;
        }
    }
}
