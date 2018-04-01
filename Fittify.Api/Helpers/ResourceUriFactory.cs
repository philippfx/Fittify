using System;
using System.Reflection;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Helpers
{
    public class ResourceUriFactory
    {
        public static string CreateResourceUriForIResourceParameters(
            IResourceParameters resourceParameters,
            IUrlHelper urlHelper,
            ResourceUriType type,
            string shortPascalCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortPascalCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            //searchQuery = resourceParameters.SearchQuery,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortPascalCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            //searchQuery = resourceParameters.SearchQuery,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortPascalCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            //searchQuery = resourceParameters.SearchQuery,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }

        public static string CreateResourceUriForISearchQueryResourceParameters(
           ISearchQueryResourceParameters resourceParameters,
           IUrlHelper urlHelper,
           ResourceUriType type,
           string shortCamelCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            searchQuery = resourceParameters.SearchQuery,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            searchQuery = resourceParameters.SearchQuery,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            searchQuery = resourceParameters.SearchQuery,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }

        public static string CreateResourceUriForISearchQueryDateTimeStartEnd(
           IDateTimeStartEndResourceParameters resourceParameters,
           IUrlHelper urlHelper,
           ResourceUriType type,
           string shortCamelCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            dateTimeStart = resourceParameters.DateTimeStart,
                            dateTimeEnd = resourceParameters.DateTimeEnd,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            dateTimeStart = resourceParameters.DateTimeStart,
                            dateTimeEnd = resourceParameters.DateTimeEnd,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            dateTimeStart = resourceParameters.DateTimeStart,
                            dateTimeEnd = resourceParameters.DateTimeEnd,
                            //genre = resourceParameters.Genre,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }

        public static string CreateResourceUriForCardioSet(
          CardioSetResourceParameters resourceParameters,
          IUrlHelper urlHelper,
          ResourceUriType type,
          string shortCamelCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            exerciseHistoryId = resourceParameters.ExerciseHistoryId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize,

                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            exerciseHistoryId = resourceParameters.ExerciseHistoryId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            exerciseHistoryId = resourceParameters.ExerciseHistoryId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }

        public static string CreateResourceUriForCategory(
          CategoryResourceParameters resourceParameters,
          IUrlHelper urlHelper,
          ResourceUriType type,
          string shortCamelCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize,

                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }

        public static string CreateResourceUriForExercise(
          ExerciseResourceParameters resourceParameters,
          IUrlHelper urlHelper,
          ResourceUriType type,
          string shortCamelCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize,

                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }

        public static string CreateResourceUriForExerciseHistory(
          ExerciseHistoryResourceParameters resourceParameters,
          IUrlHelper urlHelper,
          ResourceUriType type,
          string shortCamelCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            exerciseId = resourceParameters.ExerciseId,
                            workoutHistoryId = resourceParameters.WorkoutHistoryId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize,

                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            exerciseId = resourceParameters.ExerciseId,
                            workoutHistoryId = resourceParameters.WorkoutHistoryId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            exerciseId = resourceParameters.ExerciseId,
                            workoutHistoryId = resourceParameters.WorkoutHistoryId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }

        public static string CreateResourceUriForMapExerciseWorkout(
          MapExerciseWorkoutResourceParameters resourceParameters,
          IUrlHelper urlHelper,
          ResourceUriType type,
          string shortCamelCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            exerciseId = resourceParameters.ExerciseId,
                            workoutId = resourceParameters.WorkoutId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize,

                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            exerciseId = resourceParameters.ExerciseId,
                            workoutId = resourceParameters.WorkoutId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            exerciseId = resourceParameters.ExerciseId,
                            workoutId = resourceParameters.WorkoutId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }

        public static string CreateResourceUriForWeightLiftingSet(
           WeightLiftingSetResourceParameters resourceParameters,
           IUrlHelper urlHelper,
           ResourceUriType type,
           string shortCamelCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            exerciseHistoryId = resourceParameters.ExerciseHistoryId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize,

                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            exerciseHistoryId = resourceParameters.ExerciseHistoryId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            exerciseHistoryId = resourceParameters.ExerciseHistoryId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }

        public static string CreateResourceUriForWorkout(
           WorkoutResourceParameters resourceParameters,
           IUrlHelper urlHelper,
           ResourceUriType type,
           string shortCamelCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            category = resourceParameters.CategoryId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize,

                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            category = resourceParameters.CategoryId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            category = resourceParameters.CategoryId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }

        public static string CreateResourceUriForWorkoutHistory(
           WorkoutHistoryResourceParameters resourceParameters,
           IUrlHelper urlHelper,
           ResourceUriType type,
           string shortCamelCasedControllerName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            workoutId = resourceParameters.WorkoutId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            dateTimeStart = resourceParameters.DateTimeStart,
                            dateTimeEnd = resourceParameters.DateTimeEnd,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize,

                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            workoutId = resourceParameters.WorkoutId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            dateTimeStart = resourceParameters.DateTimeStart,
                            dateTimeEnd = resourceParameters.DateTimeEnd,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("Get" + shortCamelCasedControllerName + "Collection",
                        new
                        {
                            ids = resourceParameters.Ids,
                            workoutId = resourceParameters.WorkoutId,
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            dateTimeStart = resourceParameters.DateTimeStart,
                            dateTimeEnd = resourceParameters.DateTimeEnd,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }
    }
}
