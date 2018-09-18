/***
 * Filename: Startup.cs
 * Author : ebendutoit
 * Class   : Startup
 *
 *      Configuration for pipeline and middleware per request / service
 ***/

using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Mapper_Api.Context;
using Mapper_Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Mapper_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                        builder => builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials());
            });
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                        ReferenceLoopHandling.Ignore;
            });
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(
                        new CorsAuthorizationFilterFactory(
                                "CorsPolicy"));
            });
            services.AddScoped<CourseService>();
            var connectionString =
                    Configuration.GetConnectionString("MapperContext");
            services.AddEntityFrameworkNpgsql()
                    .AddDbContext<CourseDb>(options =>
                            options.UseNpgsql(connectionString));
                            
            services.AddScoped<LocationService>();

            // get a key at https://home.openweathermap.org/api_keys
            // todo: remove key and use app settings json
            // string appKey = "643fa9db96b5c946db296ff59f39ed50";
            services.AddScoped<WeatherService>();
            services.AddTransient<CommunicationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        "default",
                        "{controller=Home}/{action=Index}/{id?}");
            });

            app.Use(async (context, next) =>
                {
                    if (context.Request.Path == "/ws")
                    {
                        if (context.WebSockets.IsWebSocketRequest)
                        {
                            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                            await  context.RequestServices.GetService<CommunicationService>().SocketHandler(context, webSocket);
                        }
                        else
                        {
                            context.Response.StatusCode = 400;
                        }
                    }
                    else
                    {
                        await next();
                    }
                });
        }
    }
}
