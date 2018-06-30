/***
 * Filename: Program.cs
 * Author : ebendutoit
 * Class   : Program
 *
 *      Configuration file for program startup
 ***/
using System;
using System.IO;
using System.Threading.Tasks;
using Mapper_Api.Context;
using Mapper_Api.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mapper_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            RunAsync(args).GetAwaiter().GetResult();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                    .UseConfiguration(
                            new ConfigurationBuilder()
                                    .SetBasePath(
                                            Directory.GetCurrentDirectory())
                                    .AddJsonFile("hosting.json", true)
                                    .Build())
                    .UseStartup<Startup>()
                    .Build();
        }

        private static async Task RunAsync(string[] args)
        {
            var env =
                    Environment.GetEnvironmentVariable(
                            "ASPNETCORE_ENVIRONMENT");

            var serviceProvider = new ServiceCollection()
                    .AddLogging()
                    .AddDbContext<CourseDb>()
                    .AddScoped<CourseService>()
                    .BuildServiceProvider();
        }
    }
}
