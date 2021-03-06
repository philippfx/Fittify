﻿using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fittify.Api.Test.TestHelpers
{
    public class ApiTestServerStartup : Startup
    {
        public ApiTestServerStartup(IConfiguration configuration, IHostingEnvironment env) : base(configuration, env)
        {
            
        }


        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<AuthenticatedTestRequestMiddleware>();
            base.Configure(app, env, loggerFactory);
        }
    }
}
