using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fittify.Api.Helpers.ObjectResults
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        [ExcludeFromCodeCoverage]
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