﻿using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class CategoryViewModel : CategoryOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }
        //public string Name { get; set; }
    }
}