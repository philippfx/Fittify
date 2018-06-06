using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace Fittify.Client.ViewModelRepository
{
    public interface IViewModelRepository<TId, TViewModel, TOfmForPost, TResourceParameters, TGetCollectionResourceParameters>
        where TId : struct
        where TViewModel : class
        where TOfmForPost : class
        where TResourceParameters : class, new()
        where TGetCollectionResourceParameters : class, new()
    {
        Task<ViewModelQueryResult<TViewModel>> GetById(TId id);
        Task<ViewModelQueryResult<TViewModel>> GetById(TId id, TResourceParameters resourceParameters);
        Task<ViewModelCollectionQueryResult<TViewModel>> GetCollection(TGetCollectionResourceParameters resourceParameters);
        Task<ViewModelQueryResult<TViewModel>> Create(TOfmForPost workoutOfmForPost);
        Task<ViewModelQueryResult<TViewModel>> Delete(TId id);
        Task<ViewModelQueryResult<TViewModel>> PartiallyUpdate(TId id, JsonPatchDocument jsonPatchDocument);
    }
}