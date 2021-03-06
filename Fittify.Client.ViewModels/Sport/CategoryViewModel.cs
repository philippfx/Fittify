﻿using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Client.ViewModels.Sport
{
    public class CategoryViewModel : CategoryOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }
    }
}
