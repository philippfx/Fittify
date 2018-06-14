using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Quantus.IDP.Helpers.ObjectResults
{
    public class UnprocessableEntityObjectResult : ObjectResult
    {
        [ExcludeFromCodeCoverage]
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
