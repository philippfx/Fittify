using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Fittify.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fittify
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    var host = new WebHostBuilder()
        //        .UseKestrel()
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .UseIISIntegration()
        //        .UseStartup<Startup>()
        //        .UseApplicationInsights()
        //        .Build();

        //    host.Run();
        //}

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        // Only used by EF Tooling
        //public static IWebHost BuildWebHost(string[] args)
        //{
        //    return WebHost.CreateDefaultBuilder()
        //        .ConfigureAppConfiguration((ctx, cfg) =>
        //        {
        //            cfg.SetBasePath(Directory.GetCurrentDirectory())
        //                .AddJsonFile("appsettings.json", true) // require the json file!
        //                .AddEnvironmentVariables();
        //        })
        //        .ConfigureLogging((ctx, logging) => { }) // No logging
        //        .UseStartup<Startup>()
        //        .UseSetting("DesignTime", "true")
        //        .Build();
        //}
    }
    
}
