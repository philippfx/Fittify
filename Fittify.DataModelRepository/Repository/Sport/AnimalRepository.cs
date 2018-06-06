using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepository.Repository.Sport
{
    [ExcludeFromCodeCoverage] // Test for generic controller
    public class AnimalRepository : AsyncCrudBase<Animal, int>, IAsyncEntityOwnerIntId
    {
        public AnimalRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public async Task<PagedList<Animal>> GetPagedCollection(AnimalResourceParameters ofmResourceParameters)
        {
            return await Task.Run(() =>
            {
                return new PagedList<Animal>(new List<Animal> {new Animal() {Id = 1, Name = "someName"}}, 5, 5, 5);
            });
        }
    }
}