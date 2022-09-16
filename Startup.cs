using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace aspnet_core_secrets
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }
        public static IConfigurationRoot Configuration { get; set; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.common.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("secrets/appsettings.secrets.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            //loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                var message = $"Host: {Environment.MachineName}\n" +
                    $"EnvironmentName: {env.EnvironmentName}\n" +
                    $"FromEmail value (env specific): {Configuration["Email:FromAddress"]}\n" +
                    $"ConnectionString value: {Configuration["Database:ConnectionString"]}\n" +
                    $"Common/Static Cache value: {Configuration["Cache:ConnectionString"]}";
                await context.Response.WriteAsync(message);
            });
        }
    }
}
