using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModelRepository.Sport;
using Moq;
using NUnit.Framework;

namespace Fittify.Client.ViewModelRepository.Test.Sport
{
    [TestFixture]
    class ExerciseHistoryViewModelRepositoryShould
    {
        [Test]
        public async Task InstantiateCorrectly()
        {
            await Task.Run(() =>
            {
                var exerciseHistoryApiModelRepositoryMock = new Mock<IApiModelRepository<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmCollectionResourceParameters>>();

                var exerciseHistoryViewModelRepository = new ExerciseHistoryViewModelRepository(exerciseHistoryApiModelRepositoryMock.Object);

                Assert.IsNotNull(exerciseHistoryViewModelRepository);
            });
        }
    }
}
