using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;

namespace Fittify.Api.Helpers.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class EnvironmentExtensions
    {
        public static bool IsTestInMemoryDb(this IHostingEnvironment env)
        {
            return env.IsEnvironment(StandardEnvironment.TestInMemoryDb);
        }
    }
}
