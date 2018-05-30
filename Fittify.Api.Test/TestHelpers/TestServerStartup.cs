using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Fittify.Api.Test.TestHelpers
{
    class TestServerStartup : Startup
    {
        public TestServerStartup(IHostingEnvironment env) : base(env)
        {

        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<AuthenticatedTestRequestMiddleware>();
            base.Configure(app, env, loggerFactory);
        }
    }
}
