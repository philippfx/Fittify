using System;
using System.Diagnostics.CodeAnalysis;

namespace Fittify.Common.CustomExceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class OpenIdConnectException : Exception
    {
        public OpenIdConnectException()
        { }

        public OpenIdConnectException(string message)
            : base(message)
        { }

        public OpenIdConnectException(string message, Exception innerException)
            : base(message, innerException)
        { }
        
    }
}
