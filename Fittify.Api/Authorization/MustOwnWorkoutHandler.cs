using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Repository.Sport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fittify.Api.Authorization
{
    public class MustOwnWorkoutHandler : AuthorizationHandler<MustOwnWorkoutRequirement>
    {
        private WorkoutRepository _workoutRepository;

        public MustOwnWorkoutHandler(FittifyContext fittifyContext)
        {
            _workoutRepository = new WorkoutRepository(fittifyContext);
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
            var workoutIdString = filterContext.RouteData.Values["id"].ToString();
            int workoutId;
            if (!int.TryParse(workoutIdString, out workoutId))
            {
                context.Fail();
                return Task.FromResult(0);
            }

            // get subjectId
            var ownerIdString = context.User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            Guid ownerGuid;
            if (!Guid.TryParse(ownerIdString, out ownerGuid))
            {
                context.Fail();
                return Task.FromResult(0);
            }

            if (!_workoutRepository.IsEntityOwner(workoutId, ownerGuid))
            {
                context.Fail();
                return Task.FromResult(0);
            }

            context.Succeed(requirement);
            return Task.FromResult(0);
        }
    }
}
