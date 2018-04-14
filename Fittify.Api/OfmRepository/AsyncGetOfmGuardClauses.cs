using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Common;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Services;

namespace Fittify.Api.OfmRepository
{
    /// <summary>
    /// Sealed because this guard clause can be used for any incoming request.
    /// For example, the "orderBy" and "fields" queries from the request uri are checked against the TOfmForGet and not any other class
    /// </summary>
    /// <typeparam name="TOfmForGet"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public sealed class AsyncGetOfmGuardClauses<TOfmForGet, TId>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TId : struct
    {
        private readonly ITypeHelperService _typeHelperService;
        public AsyncGetOfmGuardClauses(ITypeHelperService typeHelperService)
        {
            _typeHelperService = typeHelperService;
        }
        public async Task<OfmForGetQueryResult<TOfmForGet>> ValidateGetById(OfmForGetQueryResult<TOfmForGet> ofmForGetResult, string fields)
        {
            await Task.Run(() =>
            {
                var errorMessages = new List<string>();
                if (!_typeHelperService.TypeHasProperties<TOfmForGet>(fields, ref errorMessages))
                {
                    ofmForGetResult.ErrorMessages.AddRange(errorMessages);
                }
            });
            return ofmForGetResult;
        }

        public async Task<OfmForGetCollectionQueryResult<TOfmForGet>> ValidateResourceParameters(OfmForGetCollectionQueryResult<TOfmForGet> ofmForGetCollectionQueryResult, IResourceParameters resourceParameters)
        {
            await Task.Run(() =>
            {
                var errorMessages = new List<string>();
                
                var idsAreCorrectlySyntaxed = new ValidRegularExpressionRangeOfIntIdsAttribute(FittifyRegularExpressions.RangeOfIntIds);
                if (!idsAreCorrectlySyntaxed.IsValid(resourceParameters.Ids))
                {
                    ofmForGetCollectionQueryResult.ErrorMessages.Add(idsAreCorrectlySyntaxed.FormatErrorMessage(null));
                }

                var idsInAscendingOrderValidation = new ValidAscendingOrderRangeOfIntIdsAttribute();
                if (!idsInAscendingOrderValidation.IsValid(resourceParameters.Ids))
                {
                    ofmForGetCollectionQueryResult.ErrorMessages.Add(idsInAscendingOrderValidation.FormatErrorMessage(null));
                }

                errorMessages = new List<string>();
                if (!_typeHelperService.TypeHasProperties<TOfmForGet>(resourceParameters.OrderBy, ref errorMessages))
                {
                    ofmForGetCollectionQueryResult.ErrorMessages.AddRange(errorMessages);
                }

                errorMessages = new List<string>();
                if (!_typeHelperService.TypeHasProperties<TOfmForGet>(resourceParameters.Fields, ref errorMessages))
                {
                    ofmForGetCollectionQueryResult.ErrorMessages.AddRange(errorMessages);
                }
            });

            return ofmForGetCollectionQueryResult;
        }
    }
}
