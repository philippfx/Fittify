using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Fittify.Api.Extensions
{
    public static class EnvironmentExtensions
    {
        public static bool IsTestInMemoryDb(this IHostingEnvironment env)
        {
            return env.IsEnvironment(StandardEnvironment.TestInMemoryDb);
        }
    }
}
