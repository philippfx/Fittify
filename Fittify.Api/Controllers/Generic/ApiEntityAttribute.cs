using System;

namespace Fittify.Api.Controllers.Generic
{
    /// <summary>
    /// This is just a marker attribute used to allow us to identify which entities to expose in the API
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ApiEntityAttribute : Attribute
    {
    }
}
