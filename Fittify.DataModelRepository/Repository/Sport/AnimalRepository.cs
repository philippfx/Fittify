using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class AnimalRepository : AsyncCrudBase<Animal, int, AnimalResourceParameters>, IAsyncOwnerIntId
    {
        public AnimalRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override async Task<PagedList<Animal>> GetPagedCollection(AnimalResourceParameters ofmResourceParameters)
        {
            return await Task.Run(() =>
            {
                return new PagedList<Animal>(new List<Animal> {new Animal() {Id = 1, Name = "someName"}}, 5, 5, 5);
            });
        }
    }
}