using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;

namespace Fittify.Web.View.Helpers.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class EnvironmentExtensions
    {
        public static bool IsClientTestServer(this IHostingEnvironment env)
        {
            return env.IsEnvironment(StandardEnvironment.ClientTestServer);
        }
    }
}