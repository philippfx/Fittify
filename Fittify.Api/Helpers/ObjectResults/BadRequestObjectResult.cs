using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fittify.Api.Helpers.ObjectResults
{
    public class BadRequestObjectResult : ObjectResult
    {
        [ExcludeFromCodeCoverage]
        public BadRequestObjectResult(ModelStateDictionary modelState)
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }
            StatusCode = 400;
        }
    }
}