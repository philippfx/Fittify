﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fittify.Web.ApiModelRepositories
{
    public interface IAsyncGet<TReceived>
        where TReceived : class
    {
        Task<TReceived> GetSingle();
        Task<IEnumerable<TReceived>> GetCollection();
    }
}