using System;

namespace Fittify.Common.CustomExceptions
{
    [Serializable]
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