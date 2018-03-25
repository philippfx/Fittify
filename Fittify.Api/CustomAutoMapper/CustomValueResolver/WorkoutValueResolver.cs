using System.Collections.Generic;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Common.Helpers;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Api.CustomAutoMapper.CustomValueResolver
{
    public class WorkoutValueResolver : IValueResolver<string, CategoryOfmForPatch, List<Workout>>
    {
        public List<Workout> Resolve(string rangeOfIds, CategoryOfmForPatch destination, List<Workout> destinationMember, ResolutionContext context)
        {
            var listIds = RangeString.ToCollectionOfId(rangeOfIds);
            foreach (var id in listIds)
            {
                destinationMember.Add(new Workout() { Id = id});
            }

            return destinationMember;
        }
    }

    public interface IValueResolver<in TSource, in TDestination, TDestMember>
    {
        TDestMember Resolve(TSource source, TDestination destination, TDestMember destMember, ResolutionContext context);
    }
}
