using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Quantus.IDP.DataModelRepository
{
    public class QuantusUserProfileService : IProfileService
    {
        private readonly IQuantusUserRepository _marvinUserRepository;

        public QuantusUserProfileService(IQuantusUserRepository marvinUserRepository)
        {
            _marvinUserRepository = marvinUserRepository;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            Guid.TryParse(context.Subject.GetSubjectId(), out var subjectId);
            var claimsForUser = _marvinUserRepository.GetUserClaimsBySubjectId(subjectId);

            context.IssuedClaims = claimsForUser.Select
                (c => new Claim(c.ClaimType, c.ClaimValue)).ToList();

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            Guid.TryParse(context.Subject.GetSubjectId(), out var subjectId);
            context.IsActive = _marvinUserRepository.IsUserActive(subjectId);

            return Task.FromResult(0);
        }
    }
}
