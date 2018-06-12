using System.Diagnostics.CodeAnalysis;

namespace Fittify.Api.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class StandardEnvironment
    {
        public static readonly string TestInMemoryDb = "TestInMemoryDb";
        public static readonly string NoDatabase = "NoDatabase";
    }
}
