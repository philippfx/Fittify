﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.HttpMethodInterfaces
{
    public interface IAsyncGetMoreForHttpForIntId<T> where T: class
    {
        Task<ICollection<T>> GetByRangeOfIds(string inputStringForRangeOfIds);
    }
}