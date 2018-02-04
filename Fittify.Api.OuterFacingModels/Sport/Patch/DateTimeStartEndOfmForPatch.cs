﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Patch
{
    public class DateTimeStartEndOfmForPatch : DateTimeStartEndOfmBase, IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }
    }
}