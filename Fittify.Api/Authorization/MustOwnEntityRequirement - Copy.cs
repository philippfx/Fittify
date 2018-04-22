using Microsoft.AspNetCore.Authorization;

namespace Fittify.Api.Authorization
{
    public class MustOwnEntityRequirement : IAuthorizationRequirement
    {
    }
}
