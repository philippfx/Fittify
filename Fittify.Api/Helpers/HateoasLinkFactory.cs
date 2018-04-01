using System.Collections.Generic;
using Fittify.Api.Extensions;
using Fittify.Api.OuterFacingModels;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Helpers
{
    public class HateoasLinkFactory<TId>
        where TId : struct
    {
        protected readonly IUrlHelper UrlHelper;
        protected readonly string ShortPascalCasedControllerName;
        protected readonly string ShortCamelCasedControllerName;
        public HateoasLinkFactory(IUrlHelper urlHelper, string controllerName)
        {
            UrlHelper = urlHelper;
            ShortPascalCasedControllerName = controllerName.ToShortPascalCasedControllerNameOrDefault();
            ShortCamelCasedControllerName = controllerName.ToShortCamelCasedControllerNameOrDefault();
        }
        
        public IEnumerable<HateoasLink> CreateLinksForOfmForGet(TId id, string fields)
        {
            var links = new List<HateoasLink>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new HateoasLink(UrlHelper.Link("Get" + ShortPascalCasedControllerName + "ById", new { id = id }),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add(
                    new HateoasLink(UrlHelper.Link("Get" + ShortPascalCasedControllerName + "ById", new { id = id, fields = fields }),
                        "self",
                        "GET"));
            }
            
            links.Add(
                new HateoasLink(UrlHelper.Link("Create" + ShortPascalCasedControllerName, null),
                    "create_" + ShortCamelCasedControllerName,
                    "POST"));

            links.Add(
                new HateoasLink(UrlHelper.Link("PartiallyUpdate" + ShortPascalCasedControllerName,
                        new { id = id }),
                    "partially_update_" + ShortCamelCasedControllerName,
                    "PATCH"));

            links.Add(
                new HateoasLink(UrlHelper.Link("Delete" + ShortPascalCasedControllerName, new { id = id }),
                    "delete_" + ShortCamelCasedControllerName,
                    "DELETE"));


            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmForGetCollectionQuery(
            IResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForIResourceParameters(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmGetCollectionQueryIncludeByNameSearch(
            ISearchQueryResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForISearchQueryResourceParameters(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmGetCollectionQueryIncludeByDateTimeStartEnd(
            IDateTimeStartEndResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForISearchQueryDateTimeStartEnd(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForISearchQueryDateTimeStartEnd(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForISearchQueryDateTimeStartEnd(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmGetForCardioSet(
            CardioSetResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForCardioSet(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForCardioSet(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForCardioSet(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmGetForCategory(
            CategoryResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForCategory(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForCategory(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForCategory(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmGetForExercise(
            ExerciseResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForExercise(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForExercise(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForExercise(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmGetForExerciseHistory(
            ExerciseHistoryResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForExerciseHistory(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForExerciseHistory(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForExerciseHistory(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmGetForMapExerciseWorkout(
            MapExerciseWorkoutResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForMapExerciseWorkout(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForMapExerciseWorkout(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForMapExerciseWorkout(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmGetForWeightLiftingSet(
            WeightLiftingSetResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForWeightLiftingSet(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForWeightLiftingSet(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForWeightLiftingSet(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmGetForWorkout(
            WorkoutResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForWorkout(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForWorkout(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForWorkout(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            return links;
        }

        public IEnumerable<HateoasLink> CreateLinksForOfmGetForWorkoutHistory(
            WorkoutHistoryResourceParameters resourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<HateoasLink>();

            // self 
            links.Add(
                new HateoasLink(ResourceUriFactory.CreateResourceUriForWorkoutHistory(resourceParameters, UrlHelper, ResourceUriType.Current, ShortPascalCasedControllerName)
                    , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForWorkoutHistory(resourceParameters, UrlHelper,
                            ResourceUriType.NextPage, ShortPascalCasedControllerName),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new HateoasLink(ResourceUriFactory.CreateResourceUriForWorkoutHistory(resourceParameters, UrlHelper,
                            ResourceUriType.PreviousPage, ShortPascalCasedControllerName),
                        "previousPage", "GET"));
            }

            return links;
        }
    }
}
