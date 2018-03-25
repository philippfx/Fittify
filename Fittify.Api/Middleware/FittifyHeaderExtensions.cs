using Microsoft.AspNetCore.Builder;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Fittify.Api.Middleware
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class FittifyHeaderExtensions
    {
        public static IApplicationBuilder UseFittifyHeaderValidation(this IApplicationBuilder builder, IConfiguration configuration)
        {
            return builder.UseMiddleware<HeaderValidation>(configuration);
        }

        public static IApplicationBuilder EnsureFittifyHeaderDefaultValues(this IApplicationBuilder builder, IConfiguration configuration)
        {
            return builder.UseMiddleware<EnsureHeaderDefaultValues>(configuration);
        }
    }
}
