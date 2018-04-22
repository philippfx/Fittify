using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Helpers.Extensions;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Owned;
using Fittify.DataModelRepositories.Repository.Sport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fittify.Api.Authorization
{
    public class MustOwnEntityIntIdHandler : AuthorizationHandler<MustOwnEntityIntIdRequirement>
    {
        public object _entityRepository;

        public MustOwnEntityIntIdHandler(Type dynamicType,
            params object[] constructorArguments)
        {
            _entityRepository = Activator.CreateInstance(dynamicType, constructorArguments);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustOwnEntityIntIdRequirement requirement)
        {
            var filterContext = context.Resource as AuthorizationFilterContext;
            if (filterContext == null)
            {
                context.Fail();
                return Task.FromResult(0);
            }

            // getWorkoutId
            var idString = filterContext.RouteData.Values["id"].ToString();
            int entityId;
            if (!int.TryParse(idString, out entityId))
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

            var _myEntityRepository = _entityRepository as IAsyncOwnerIntId;
            if (_myEntityRepository.IsEntityOwner(entityId, ownerGuid))
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
