using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Client.ApiModelRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ViewModelRepository
{
    public class GenericViewModelRepository<TId, TViewModel, TOfmForGet, TOfmForPost, TGetCollectionResourceParameters>
        where TId : struct
        where TViewModel : class
        where TOfmForGet : class
        where TGetCollectionResourceParameters : class, new()
        where TOfmForPost : class
    {
        protected readonly GenericAsyncGppdOfm<TId, TOfmForGet, TOfmForPost, TGetCollectionResourceParameters> GenericAsyncGppdOfmWorkout;
        protected readonly IHttpContextAccessor HttpContextAccessor;
        protected readonly IConfiguration AppConfiguration;
        protected readonly IHttpRequestHandler HttpRequestHandler;

        public GenericViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor, string mappedControllerActionKey, IHttpRequestHandler httpRequestHandler)
        {
            HttpRequestHandler = httpRequestHandler;
            GenericAsyncGppdOfmWorkout = new GenericAsyncGppdOfm<TId, TOfmForGet, TOfmForPost, TGetCollectionResourceParameters>(appConfiguration, httpContextAccessor, mappedControllerActionKey, HttpRequestHandler);
            HttpContextAccessor = httpContextAccessor;
            AppConfiguration = appConfiguration;
        }

        public virtual async Task<ViewModelQueryResult<TViewModel>> GetById(TId id)
        {

            var ofmQueryResult = await GenericAsyncGppdOfmWorkout.GetSingle(id);

            var workoutViewModelQueryResult = new ViewModelQueryResult<TViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int)ofmQueryResult.HttpStatusCode == 200)
            {
                workoutViewModelQueryResult.ViewModel =
                    Mapper.Map<TViewModel>(ofmQueryResult.OfmForGet);
            }
            else
            {
                workoutViewModelQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
            }
            
            return workoutViewModelQueryResult;
        }

        public virtual async Task<ViewModelQueryResult<TViewModel>> GetById<TResourceParameters>(TId id, TResourceParameters resourceParameters) where TResourceParameters : class
        {
            var ofmQueryResult = await GenericAsyncGppdOfmWorkout.GetSingle(id, resourceParameters);

            var workoutViewModelQueryResult = new ViewModelQueryResult<TViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int)ofmQueryResult.HttpStatusCode == 200)
            {
                workoutViewModelQueryResult.ViewModel =
                    Mapper.Map<TViewModel>(ofmQueryResult.OfmForGet);
            }
            else
            {
                workoutViewModelQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
            }

            return workoutViewModelQueryResult;
        }

        public virtual async Task<ViewModelCollectionQueryResult<TViewModel>> GetCollection(TGetCollectionResourceParameters resourceParameters)
        {
            var ofmCollectionQueryResult = await GenericAsyncGppdOfmWorkout.GetCollection(resourceParameters);

            var workoutViewModelCollectionQueryResult = new ViewModelCollectionQueryResult<TViewModel>();
            workoutViewModelCollectionQueryResult.HttpStatusCode = ofmCollectionQueryResult.HttpStatusCode;

            if ((int)ofmCollectionQueryResult.HttpStatusCode == 200)
            {
                workoutViewModelCollectionQueryResult.ViewModelForGetCollection =
                    Mapper.Map<IEnumerable<TViewModel>>(ofmCollectionQueryResult.OfmForGetCollection);
            }
            else
            {
                workoutViewModelCollectionQueryResult.ErrorMessagesPresented = ofmCollectionQueryResult.ErrorMessagesPresented;
            }

            return workoutViewModelCollectionQueryResult;
        }

        public virtual async Task<ViewModelQueryResult<TViewModel>> Create(TOfmForPost workoutOfmForPost)
        {
            var ofmQueryResult = await GenericAsyncGppdOfmWorkout.Post(workoutOfmForPost);

            var workoutViewModelQueryResult = new ViewModelQueryResult<TViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int)ofmQueryResult.HttpStatusCode == 201)
            {
                workoutViewModelQueryResult.ViewModel =
                    Mapper.Map<TViewModel>(ofmQueryResult.OfmForGet);
            }
            else
            {
                ofmQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
            }

            return workoutViewModelQueryResult;
        }

        public virtual async Task<ViewModelQueryResult<TViewModel>> Delete(TId id)
        {
            var ofmQueryResult = await GenericAsyncGppdOfmWorkout.Delete(id);

            var workoutViewModelQueryResult = new ViewModelQueryResult<TViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int)ofmQueryResult.HttpStatusCode == 204)
            {
                workoutViewModelQueryResult.ViewModel =
                    Mapper.Map<TViewModel>(ofmQueryResult.OfmForGet);
            }
            else
            {
                ofmQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
            }

            return workoutViewModelQueryResult;
        }

        public virtual async Task<ViewModelQueryResult<TViewModel>> PartiallyUpdate(TId id, JsonPatchDocument jsonPatchDocument)
        {
            var ofmQueryResult = await GenericAsyncGppdOfmWorkout.Patch(id, jsonPatchDocument);

            var workoutViewModelQueryResult = new ViewModelQueryResult<TViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int)ofmQueryResult.HttpStatusCode == 201)
            {
                workoutViewModelQueryResult.ViewModel =
                    Mapper.Map<TViewModel>(ofmQueryResult.OfmForGet);
            }
            else
            {
                ofmQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
            }

            return workoutViewModelQueryResult;
        }
    }
}
