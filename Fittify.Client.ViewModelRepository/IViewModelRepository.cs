using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace Fittify.Client.ViewModelRepository
{
    public interface IViewModelRepository<TId, TViewModel, TOfmForPost, TGetCollectionResourceParameters>
        where TId : struct
        where TViewModel : class
        where TOfmForPost : class
        where TGetCollectionResourceParameters : class, new()
    {
        Task<ViewModelQueryResult<TViewModel>> GetById(TId id);
        Task<ViewModelQueryResult<TViewModel>> GetById<TResourceParameters>(TId id, TResourceParameters resourceParameters) where TResourceParameters : class;
        Task<ViewModelCollectionQueryResult<TViewModel>> GetCollection(TGetCollectionResourceParameters resourceParameters);
        Task<ViewModelQueryResult<TViewModel>> Create(TOfmForPost workoutOfmForPost);
        Task<ViewModelQueryResult<TViewModel>> Delete(TId id);
        Task<ViewModelQueryResult<TViewModel>> PartiallyUpdate(TId id, JsonPatchDocument jsonPatchDocument);
    }
}