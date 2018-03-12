using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Api.OuterFacingModels;
using Fittify.Api.Services;
using Fittify.Common;

namespace Fittify.Api.OfmRepository
{
    public class AsyncGetOfmGuardClauses<TOfmForGet, TId>
        where TOfmForGet : LinkedResourceBase, IEntityUniqueIdentifier<TId>
        where TId : struct
    {

        protected readonly ITypeHelperService TypeHelperService;
        public AsyncGetOfmGuardClauses(ITypeHelperService typeHelperService)
        {
            TypeHelperService = typeHelperService;
        }
        public virtual async Task<OfmForGetQueryResult<TOfmForGet>> ValidateGetByIdInput(OfmForGetQueryResult<TOfmForGet> ofmForGetResult, string fields)
        {
            ofmForGetResult.ErrorMessages = new List<string>();
            IList<string> errorMessages = new List<string>();

            await Task.Run(() =>
            {
                if (!TypeHelperService.TypeHasProperties<TOfmForGet>(fields, ref errorMessages))
                {
                    ofmForGetResult.ErrorMessages.AddRange(errorMessages);
                }
            });

            return ofmForGetResult;
        }
    }
}
