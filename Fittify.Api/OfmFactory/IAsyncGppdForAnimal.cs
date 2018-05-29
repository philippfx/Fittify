using System;
using System.Diagnostics.CodeAnalysis;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd.Sport;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.Services;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common;
using Fittify.DataModelRepository;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.Repository.Sport.Factory;
using Fittify.DataModelRepository.ResourceParameters;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.Extensions.DependencyInjection;


namespace Fittify.Api.OfmFactory
{
    public interface IAsyncGppdForAnimal : IAsyncGppd<AnimalOfmForGet, AnimalOfmForPost, AnimalOfmForPatch, int, AnimalOfmResourceParameters>
    {

    }

    //public class AnimalOfmRepository : AsyncGppdBase<Animal, AnimalOfmForGet, AnimalOfmForPost, AnimalOfmForPatch, int, AnimalOfmResourceParameters, AnimalResourceParameters>
    //{
    //    public AnimalOfmRepository(IAsyncCrud<Animal, int, AnimalResourceParameters> repo,
    //        IPropertyMappingService propertyMappingService,
    //        ITypeHelperService typeHelperService
    //    )
    //        : base(repo, propertyMappingService, typeHelperService)
    //    {
    //    }
    //}

    [ExcludeFromCodeCoverage]
    public class OfmRepository<T> : AsyncGppdBase<Animal, AnimalOfmForGet, AnimalOfmForPost, AnimalOfmForPatch, int, AnimalOfmResourceParameters, AnimalResourceParameters>
        where T : class
    {
        public OfmRepository(IAsyncCrud<Animal, int, AnimalResourceParameters> repo,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService
        )
            : base(repo, propertyMappingService, typeHelperService)
        {
        }
    }

    public interface IAsyncGppd<T> : IAsyncGppd<AnimalOfmForGet, AnimalOfmForPost, AnimalOfmForPatch, int, AnimalOfmResourceParameters>
        where T: class
    {
    }

    [ExcludeFromCodeCoverage]
    public abstract class AsyncGppdFactory<TEntity, TOfmForGet, TOfmForPost, TOfmForPatch, TId, TOfmResourceParameters, TEntityResourceParameters>
        //: IAsyncGppd<TOfmForGet, TOfmForPost, TOfmForPatch, TId, TOfmResourceParameters>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TOfmForPost : class
        where TOfmForPatch : class
        where TId : struct
        where TOfmResourceParameters : OfmResourceParametersBase
        where TEntityResourceParameters : EntityResourceParametersBase, IEntityOwner
    {
        public abstract AsyncGppdBase<TEntity, TOfmForGet, TOfmForPost, TOfmForPatch, TId, TOfmResourceParameters, TEntityResourceParameters> CreateInstance(string type);
    }

    [ExcludeFromCodeCoverage]
    public class AsyncGppdFactory<R> where R : class
    {
        private readonly IServiceProvider _services;
        public AsyncGppdFactory(IServiceProvider services)
        {
            _services = services;
        }

        public R CreateGeneric<T>()
            where T : class, IEntityName<int>
        {
            var fittifyContext = _services.GetService<FittifyContext>();

            if (typeof(T) == typeof(Fittify.Api.Controllers.Generic.Animals))
            {
                var repoFactory = new DataModelRepositoryFactory(fittifyContext);
                var animalRepo = repoFactory.CreateGeneric<Animal, int, AnimalResourceParameters>();

                var ofmRepo = (IAsyncGppd <AnimalOfmForGet, AnimalOfmForPost, AnimalOfmForPatch, int, AnimalOfmResourceParameters>)
                    new AnimalOfmRepository(animalRepo, new PropertyMappingService(), new TypeHelperService());

                return (R)ofmRepo;
            }

            //if (typeof(T) == typeof(int))
            //{
            //    return (IAsyncGppd<T>)new GenericInt();
            //}

            throw new InvalidOperationException();
        }
    }
}