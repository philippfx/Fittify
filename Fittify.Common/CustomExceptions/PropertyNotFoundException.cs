using System;
using System.Diagnostics.CodeAnalysis;

namespace Fittify.Common.CustomExceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException()
        { }

        public PropertyNotFoundException(string message)
            : base(message)
        { }

        public PropertyNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}