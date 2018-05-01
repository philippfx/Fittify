using System;

namespace Fittify.Common.CustomExceptions
{
    [Serializable]
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