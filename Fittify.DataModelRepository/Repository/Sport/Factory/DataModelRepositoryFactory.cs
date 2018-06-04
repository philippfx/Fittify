using System;
using System.Diagnostics.CodeAnalysis;

namespace Fittify.DataModelRepository.Repository.Sport.Factory
{
    [ExcludeFromCodeCoverage] // Is an experiment for generic controllers
    public class DataModelRepositoryFactory
    {
        private readonly FittifyContext _fittifyContext;
        public DataModelRepositoryFactory(FittifyContext fittifyContext)
        {
            _fittifyContext = fittifyContext;
        }

        //public IAsyncCrud<TEntity, TId, TResourceParameters> CreateGeneric<TEntity, TId, TResourceParameters>()
        //    where TEntity : class, IEntityUniqueIdentifier<TId>
        //    where TId : struct
        //    where TResourceParameters : class, IResourceParameters
        //{
        //    if (typeof(TEntity) == typeof(Animal) /*&& typeof(TId) == typeof(int) && typeof(TResourceParameters) == typeof(AnimalResourceParameters)*/)
        //    {
        //        var animalRepoInstance = (IAsyncCrud<TEntity, TId, TResourceParameters>) new AnimalRepository(_fittifyContext);
        //        return animalRepoInstance;
        //    }

        //    //if (typeof(T) == typeof(int))
        //    //{
        //    //    return (IAsyncGppd<T>)new GenericInt();
        //    //}

        //    throw new InvalidOperationException();
        //}

        public IAsyncEntityOwnerIntId CreateAsyncOwnerIntIdInstance(Type dataRepositoryType)
        {
            if (dataRepositoryType == typeof(WorkoutRepository))
            {
                return new WorkoutRepository(_fittifyContext);
            }

            return null;
        }
    }
}
