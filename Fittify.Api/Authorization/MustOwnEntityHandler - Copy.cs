using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Helpers.Extensions;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Repository.Sport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fittify.Api.Authorization
{
    public class MustOwnEntityHandler<TDataRepository, TEntity, TId> : AuthorizationHandler<MustOwnEntityRequirement>
        where TDataRepository : AsyncCrudOwned<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>, IEntityOwner
        where TId : struct
    {
        private TDataRepository _entityRepository;

        public MustOwnEntityHandler(FittifyContext fittifyContext)
        {
            _entityRepository = (TDataRepository)Activator.CreateInstance(typeof(TDataRepository), fittifyContext);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustOwnEntityRequirement requirement)
        {
            var filterContext = context.Resource as AuthorizationFilterContext;
            if (filterContext == null)
            {
                context.Fail();
                return Task.FromResult(0);
            }

            // getWorkoutId
            var idString = filterContext.RouteData.Values["id"].ToString();
            TId id = TryParse<TId>(idString);
            if (id.IsDefault())
            {
                context.Fail();
                return Task.FromResult(0);
            }

            //if (!int.TryParse(idString, out workoutId))
            //{
            //    context.Fail();
            //    return Task.FromResult(0);
            //}

            // get subjectId
            var ownerIdString = context.User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            Guid ownerGuid;
            if (!Guid.TryParse(ownerIdString, out ownerGuid))
            {
                context.Fail();
                return Task.FromResult(0);
            }

            if (_entityRepository.IsEntityOwner(id, ownerGuid))
            {
                context.Fail();
                return Task.FromResult(0);
            }

            context.Succeed(requirement);
            return Task.FromResult(0);
        }

        public static T TryParse<T>(string inValue)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof(T));

            return (T)converter.ConvertFromString(null,
                CultureInfo.InvariantCulture, inValue);
        }
    }
}
