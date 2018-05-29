using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fittify.Api.Helpers.ObjectResults
{
    /// <summary>
    /// Returns a 404 status code and a json Array with a detailed error Message
    /// </summary>
    public class EntityNotFoundObjectResult : ObjectResult
    {
        [ExcludeFromCodeCoverage]
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
