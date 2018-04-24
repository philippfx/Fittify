using Microsoft.Extensions.DependencyInjection;
using Quantus.IDP.Services;

namespace Quantus.IDP
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddQuantusUserStore(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<IQuantusUserRepository, QuantusUserRepository>();
            builder.AddProfileService<QuantusUserProfileService>();
            return builder;
        }
    }
}
