using System;
using CAP.LoggingLibrary.Builder;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TestCosmosSQL.RestApi
{
    /// <summary>
    /// Program entry point
    /// </summary>
    public class Program
    {

        /// <summary>
        /// Main Method
        /// </summary>
        /// <param name="args">Arguments</param>
        public static void Main(string[] args)
        {

            // Start Web Host Builder
            try
            {
                LoggingClientWebHostBuilderExtensions.LogInformation("Starting web host");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                LoggingClientWebHostBuilderExtensions.LogError(ex, "Host terminated unexpectedly");
                throw;
            }
            finally
            {
                LoggingClientWebHostBuilderExtensions.CloseAndFlush();

            }


        }

        /// <summary>
        /// Builds Web Host
        /// </summary>
        /// <param name="args">Arguments</param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            //for development load development.json else the secret one.
            if (environment.ToUpper() == "DEVELOPMENT")
                configurationBuilder.AddJsonFile($"appsettings.Development.json", optional: true,
                    reloadOnChange: true);
            else
                configurationBuilder.AddJsonFile("Secrets/appsettings.Kubernetes.json", optional: true, reloadOnChange: true);

            var configuration = configurationBuilder.Build();

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(d => d.ClearProviders())
                .ConfigureAppConfiguration((hostingContext, config) =>
                                    {
                                        config.AddConfiguration(configuration);

                                    })

                .UseCapLogClient(configuration)
                .UseStartup<Startup>();


        }

    }
}
