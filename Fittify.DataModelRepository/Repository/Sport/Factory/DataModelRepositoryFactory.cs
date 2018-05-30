using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using Fittify.Common;
using Fittify.Common.ResourceParameters;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;

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

        public IAsyncCrud<TEntity, TId, TResourceParameters> CreateGeneric<TEntity, TId, TResourceParameters>()
            where TEntity : class, IEntityUniqueIdentifier<TId>
            where TId : struct
            where TResourceParameters : class, IResourceParameters
        {
            if (typeof(TEntity) == typeof(Animal) /*&& typeof(TId) == typeof(int) && typeof(TResourceParameters) == typeof(AnimalResourceParameters)*/)
            {
                var animalRepoInstance = (IAsyncCrud<TEntity, TId, TResourceParameters>) new AnimalRepository(_fittifyContext);
                return animalRepoInstance;
            }

            //if (typeof(T) == typeof(int))
            //{
            //    return (IAsyncGppd<T>)new GenericInt();
            //}

            throw new InvalidOperationException();
        }
    }
}
