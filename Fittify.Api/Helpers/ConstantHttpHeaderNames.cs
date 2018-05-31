using System.Reflection;
using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("Fittify.Api.Test")]
namespace Fittify.Api.Helpers
{
    public static class ConstantHttpHeaderNames
    {
        internal const string ApiVersion = "Api-Version";
        internal const string IncludeHateoas = "Include-Hateoas";
    }
}
