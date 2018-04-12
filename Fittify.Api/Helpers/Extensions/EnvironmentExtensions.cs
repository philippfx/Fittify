using Microsoft.AspNetCore.Hosting;

namespace Fittify.Api.Helpers.Extensions
{
    public static class EnvironmentExtensions
    {
        public static bool IsTestInMemoryDb(this IHostingEnvironment env)
        {
            return env.IsEnvironment(StandardEnvironment.TestInMemoryDb);
        }
    }
}
