using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Fittify.Api.Helpers
{
    /// <summary>
    /// Returns a 404 status code and a json Array with a detailed error Message
    /// </summary>
    public class EntityNotFoundObjectResult : ObjectResult
    {
        [JsonConstructor]
        [Obsolete("Don't use this constructor. It is required for unit testing only")]
        public EntityNotFoundObjectResult() // Todo: I need this for unit tests when deserializing expected json string to this EntityNotFoundObjectResult. Find a way to deserialize with parameters!
            : base(new SerializableError(new ModelStateDictionary()))
        {
            //StatusCode = 404; // Set by desserializing object json
        }

        public EntityNotFoundObjectResult(ModelStateDictionary modelState)
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }
            StatusCode = 404;
        }
    }
}
