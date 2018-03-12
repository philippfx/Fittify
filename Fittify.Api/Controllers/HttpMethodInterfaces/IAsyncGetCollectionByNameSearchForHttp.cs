﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.HttpMethodInterfaces
{
    public interface IAsyncGetCollectionByNameSearchForHttp
    {
        Task<IActionResult> GetCollection(SearchQueryResourceParameters resourceParameters);
    }
}
