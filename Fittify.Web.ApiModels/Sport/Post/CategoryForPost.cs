﻿using System.Collections.Generic;

namespace Fittify.Web.ApiModels.Sport.Post
{
    public class CategoryForPost
    {
        public string Name { get; set; }
        public virtual IEnumerable<int> WorkoutIds { get; set; }

    }
}
