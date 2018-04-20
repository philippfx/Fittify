using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModelRepository.Sport;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository
{
    public class GenericViewModelRepository<TId, TViewModel, TOfmForGet, TOfmForPost, TResourceParameters>
        where TId : struct
        where TViewModel : class
        where TOfmForGet : class
        where TResourceParameters : class
        where TOfmForPost : class
    {
        protected readonly GenericAsyncGppdOfm<TId, TOfmForGet, TOfmForPost, TResourceParameters> GenericAsyncGppdOfmWorkout;

        public GenericViewModelRepository(IConfiguration appConfiguration, string mappedControllerActionKey)
        {
            GenericAsyncGppdOfmWorkout = new GenericAsyncGppdOfm<TId, TOfmForGet, TOfmForPost, TResourceParameters>(appConfiguration, mappedControllerActionKey);
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

        public virtual async Task<ViewModelCollectionQueryResult<TViewModel>> GetCollection(TResourceParameters resourceParameters)
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
