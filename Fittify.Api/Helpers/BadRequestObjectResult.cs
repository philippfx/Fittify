using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

public class BadRequestObjectResult : ObjectResult
{
    [JsonConstructor]
    [Obsolete("Don't use this constructor. It is required for unit testing only")]
    public BadRequestObjectResult() // Todo: I need this for unit tests when deserializing expected json string to this BadRequestObjectResult. Find a way to deserialize with parameters!
        : base(new SerializableError(new ModelStateDictionary()))
    {
        //StatusCode = 400; // Set by the deserialized object
    }

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