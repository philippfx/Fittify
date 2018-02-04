using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fittify.Api.Helpers
{
    /// <summary>
    /// Returns a 404 status code and a json Array with a detailed error Message
    /// </summary>
    public class EntityNotFoundObjectResult : ObjectResult
    {
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
