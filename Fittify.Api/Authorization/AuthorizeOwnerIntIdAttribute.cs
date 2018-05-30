using System;
using System.Linq;
using Fittify.DataModelRepository;
using Fittify.DataModelRepository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Fittify.Api.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class AuthorizeOwnerIntIdAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private object _entityRepositoryObject;
        private IAsyncOwnerIntId _entityRepository;
        private readonly Type _TCrudRepository;

        public AuthorizeOwnerIntIdAttribute(Type TCrudRepository)
        {
            _TCrudRepository = TCrudRepository;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var fittifyContext = context.HttpContext.RequestServices.GetService<FittifyContext>();
            _entityRepositoryObject = Activator.CreateInstance(_TCrudRepository, fittifyContext);
            _entityRepository = _entityRepositoryObject as IAsyncOwnerIntId;

            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                // it isn't needed to set unauthorized result 
                // as the base class already requires the user to be authenticated
                // this also makes redirect to a login page work properly
                // context.Result = new UnauthorizedResult();
                return;
            }

            // get entityId from uri
            var idString = context.RouteData.Values["id"].ToString();
            if (!int.TryParse(idString, out var entityId))
            {
                context.Result = new BadRequestResult();
                return;
            }
            
            // get subjectId from user claims
            var ownerIdString = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (!Guid.TryParse(ownerIdString, out var ownerGuid))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            if (_entityRepository != null && await _entityRepository?.IsEntityOwner(entityId, ownerGuid) == false)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
