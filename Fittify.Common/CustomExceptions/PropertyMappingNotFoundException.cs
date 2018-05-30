using System;
using System.Diagnostics.CodeAnalysis;

namespace Fittify.Common.CustomExceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class PropertyMappingNotFoundException : Exception
    {
        public PropertyMappingNotFoundException()
        { }

        public PropertyMappingNotFoundException(string message)
            : base(message)
        { }

        public PropertyMappingNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}