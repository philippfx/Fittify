﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers
{
    public interface IAsyncGetCollectionForHttp
    {
        Task<IActionResult> GetCollection(IResourceParameters resourceParameters);
    }
}
