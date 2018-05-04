using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Common.CustomExceptions
{
    [Serializable]
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
