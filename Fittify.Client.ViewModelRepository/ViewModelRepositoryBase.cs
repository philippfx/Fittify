using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Client.ApiModelRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ViewModelRepository
{
    public abstract class ViewModelRepositoryBase<TId, TViewModel, TOfmForGet, TOfmForPost, TOfmResourceParameters, TGetCollectionResourceParameters>
        : IViewModelRepository<TId, TViewModel, TOfmForPost, TOfmResourceParameters, TGetCollectionResourceParameters>
        where TId : struct
        where TViewModel : class
        where TOfmForGet : class
        where TOfmResourceParameters : class, new()
        where TGetCollectionResourceParameters : class, new()
        where TOfmForPost : class
    {
        protected readonly IApiModelRepository<TId, TOfmForGet, TOfmForPost, TGetCollectionResourceParameters> ApiModelRepository;
        ////protected readonly IHttpContextAccessor HttpContextAccessor;
        ////protected readonly IConfiguration AppConfiguration;
        ////protected readonly IHttpRequestExecuter HttpRequestExecuter;

        public ViewModelRepositoryBase(
            ////IConfiguration appConfiguration,
            ////IHttpContextAccessor httpContextAccessor,
            ////string mappedControllerActionKey,
            ////IHttpRequestExecuter httpRequestExecuter,
            IApiModelRepository<TId, TOfmForGet, TOfmForPost, TGetCollectionResourceParameters> apiModelRepository)
        {
            ApiModelRepository = apiModelRepository;
            ////HttpRequestExecuter = httpRequestExecuter;
            ////HttpContextAccessor = httpContextAccessor;
            ////AppConfiguration = appConfiguration;
        }

        public virtual async Task<ViewModelQueryResult<TViewModel>> GetById(TId id)
        {
            var ofmQueryResult = await ApiModelRepository.GetSingle(id);

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

        public virtual async Task<ViewModelQueryResult<TViewModel>> GetById(TId id, TOfmResourceParameters resourceParameters)
        {
            var ofmQueryResult = await ApiModelRepository.GetSingle(id, resourceParameters);

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
            var ofmCollectionQueryResult = await ApiModelRepository.GetCollection(resourceParameters);

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

        public virtual async Task<ViewModelQueryResult<TViewModel>> Create(TOfmForPost workoutHistoryOfmForPost)
        {
            var ofmQueryResult = await ApiModelRepository.Post(workoutHistoryOfmForPost);

            var workoutViewModelQueryResult = new ViewModelQueryResult<TViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int)ofmQueryResult.HttpStatusCode == 201)
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

        public virtual async Task<ViewModelQueryResult<TViewModel>> Delete(TId id)
        {
            var ofmQueryResult = await ApiModelRepository.Delete(id);

            var workoutViewModelQueryResult = new ViewModelQueryResult<TViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int)ofmQueryResult.HttpStatusCode == 204)
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

        public virtual async Task<ViewModelQueryResult<TViewModel>> PartiallyUpdate(TId id, JsonPatchDocument jsonPatchDocument)
        {
            var ofmQueryResult = await ApiModelRepository.Patch(id, jsonPatchDocument);

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
    }
}
