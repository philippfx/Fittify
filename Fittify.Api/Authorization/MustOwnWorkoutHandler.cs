using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.DataModelRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fittify.Api.Authorization
{
    public class MustOwnWorkoutHandler : AuthorizationHandler<MustOwnWorkoutRequirement>
    {
        private FittifyContext _fittifyContext;

        public MustOwnWorkoutHandler(FittifyContext fittifyContext)
        {
            _fittifyContext = fittifyContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustOwnWorkoutRequirement requirement)
        {
            var filterContext = context.Resource as AuthorizationFilterContext;
            if (filterContext == null)
            {
                context.Fail();
                return Task.FromResult(0);
            }

            // getWorkoutId
            var workoutIdString = filterContext.RouteData.Values["Id"].ToString();
            int workoutId;
            if (!int.TryParse(workoutIdString, out workoutId))
            {
                context.Fail();
                return Task.FromResult(0);
            }

            // get subjectId
            var ownerId = context.User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

        }
    }
}
