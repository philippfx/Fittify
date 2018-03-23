using AutoMapper.Configuration;
using Microsoft.AspNetCore.Builder;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Fittify.Api.Middleware
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class FittifyHeaderValidationExtensions
    {
        public static IApplicationBuilder UseFittifyHeaderValidation(this IApplicationBuilder builder, IConfiguration configuration)
        {
            return builder.UseMiddleware<HeaderValidation>(configuration);
        }
    }
}
