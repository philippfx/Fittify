using System;
using System.Linq;
using Fittify.Api.Helpers.ObjectResults;
using Fittify.Api.OfmRepository.OfmRepository;
using Fittify.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fittify.Api.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class AuthorizeOwnerIntIdAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private IAsyncOfmOwnerIntId _ofmRepository;
        private readonly Type _ofmRepositoryType;

        public AuthorizeOwnerIntIdAttribute(Type ofmRepositoryType)
        {
            _ofmRepositoryType = ofmRepositoryType;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var ofmRepositoryObject = context.HttpContext.RequestServices.GetService(_ofmRepositoryType);
            _ofmRepository = ofmRepositoryObject as IAsyncOfmOwnerIntId;

            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
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

            if (_ofmRepository == null)
            {
                var modelStateDictionary = new ModelStateDictionary();
                modelStateDictionary.AddModelError("_ofmRepository", "_ofmRepository could not be retrieved from dependency container and must not be null");
                context.Result = new InternalServerErrorObjectResult(modelStateDictionary);
                return;
            }
            
            if (await _ofmRepository?.IsEntityOwner(entityId, ownerGuid) == false)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
