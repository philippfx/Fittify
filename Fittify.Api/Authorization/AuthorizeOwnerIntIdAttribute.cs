using System;
using System.Linq;
using Fittify.Api.OfmRepository.OfmRepository;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.Services;
using Fittify.Api.OfmRepository.Services.OfmDataRepositoryMapping;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
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
        private object _ofmRepositoryObject;
        private IAsyncOfmOwnerIntId _ofmRepository;
        private readonly Type _ofmRepositoryType;

        public AuthorizeOwnerIntIdAttribute(Type ofmRepositoryType)
        {
            _ofmRepositoryType = ofmRepositoryType;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            //var test = context.HttpContext.RequestServices.GetServices()

            //var services = context.HttpContext.RequestServices.GetServices(_ofmRepositoryType);
            //var moreServices = context.HttpContext.RequestServices.GetRequiredService(_ofmRepositoryType);
            var evenMoreServices = context.HttpContext.RequestServices.GetService(_ofmRepositoryType);


            //var servicesby = context.HttpContext.RequestServices.GetServices(typeof(IAsyncOfmRepository<WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmForPatch, int, WorkoutOfmResourceParameters>));
            //var moreServicesby = context.HttpContext.RequestServices.GetRequiredService(typeof(IAsyncOfmRepository<WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmForPatch, int, WorkoutOfmResourceParameters>));
            //var evenMoreServicesby = context.HttpContext.RequestServices.GetService(typeof(IAsyncOfmRepository<WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmForPatch, int, WorkoutOfmResourceParameters>));

            //var fittifyContext = context.HttpContext.RequestServices.GetService(typeof(FittifyContext));
            //var ofmRepositoryService = context.HttpContext.RequestServices.GetService(_ofmRepositoryType);
            //var propertyMappingService = context.HttpContext.RequestServices.GetService(typeof(IPropertyMappingService));
            //var typeHelperService = context.HttpContext.RequestServices.GetService(typeof(ITypeHelperService));
            //var ofmDataRepositoryMappingService = context.HttpContext.RequestServices.GetService(typeof(IOfmDataRepositoryMappingService)) as IOfmDataRepositoryMappingService;
            //var dataRepositoryType = ofmDataRepositoryMappingService.GetDestination(_ofmRepositoryType);
            //var dataRepository = 


            //_ofmRepositoryObject = Activator.CreateInstance(_ofmRepositoryType, new object[] { ofmRepositoryService, null, null });
            _ofmRepository = evenMoreServices as IAsyncOfmOwnerIntId;

            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                // it isn't needed to set unauthorized result 
                // as the base class already requires the user to be authenticated
                // this also makes redirect to a login page work properly
                context.Result = new UnauthorizedResult(); // I do it anyway
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
            
            if (_ofmRepository != null && await _ofmRepository?.IsEntityOwner(entityId, ownerGuid) == false)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
