using CAP.ProblemDetails.Exceptions;
using TestCosmosSQL.Application.Configuration;
using TestCosmosSQL.Application.Services;
using CorrelationId;
using HealthChecks.UI.Client;
using Hellang.Middleware.ProblemDetails;
using CAP.LoggingLibrary.Builder;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace TestCosmosSQL.RestApi
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<Startup> _logger;
        /// <summary>
        /// Configuration object
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        /// <param name="logger"></param>
        public Startup(IConfiguration configuration, IHostingEnvironment environment, ILogger<Startup> logger)
        {
            _environment = environment;
            _logger = logger;
            Configuration = configuration;
        }



        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //create logger for startup
            _logger.LogInformation("On ConfigureServices Method: TestCosmosSQL");

            //Set Compatibility Version
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //add problem details.
            services.AddCapProblemDetails(MapCustomExceptionsToKnownHttpStatusCode(), _environment.IsDevelopment());

            //Add Correlation Id
            services.AddCorrelationId();

            //Validate Configuration file
            var configuration = new TestCosmosSQLConfiguration();
            Configuration.GetSection("TestCosmosSQLConfiguration").Bind(configuration);
            configuration.Validate();


            // Add our Config object so it can be injected
            services.Configure<TestCosmosSQLConfiguration>(Configuration.GetSection("TestCosmosSQLConfiguration"));

            //Add TestCosmosSQLService
            services.AddSingleton<ITestCosmosSQLService, TestCosmosSQLService>();

            //enrich application insight with kubernetes information
            if (_environment.IsProduction() || _environment.IsStaging())
                services.AddApplicationInsightsKubernetesEnricher();

            //Add Custom properties and cloud role name
            services.AddSingleton<ITelemetryInitializer, CustomTelemetryInitializer>();

            //Add Health Check 
            //self check is an example. Add checks for dependent services.
            var healthCheckBuilder = services.AddHealthChecks();

            if (_environment.IsDevelopment())
            {
                //Add all dependent services health checks.
                healthCheckBuilder.AddCheck("self", c => HealthCheckResult.Healthy());
                //Add Health Check UI.
                services.AddHealthChecksUI();

                //Add Swagger
                services.AddSwaggerDocument(c =>
                {
                    c.PostProcess = document =>
                    {
                        document.Info.Title = "TestCosmosSQL REST API";
                        document.Info.Version = "v1";
                        document.Info.Description = "TestCosmosSQL REST API";
                        document.Info.TermsOfService = "Conduent Internal";

                        document.Info.Contact = new NSwag.SwaggerContact
                        {
                            Name = "CAP Support",
                            Email = "CAP-Support@conduent.com",
                            Url = "https://www.Conduent.com"
                        };
                        document.Info.License = new NSwag.SwaggerLicense
                        {
                            Name = "Conduent Internal only"

                        };

                    };

                });
            }


        }


        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="correlationContext"></param>
        /// <param name="logger"></param>
        /// <param name="applicationLifetime"></param>
        /// <param name="monitor"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ICorrelationContextAccessor correlationContext,
            ILogger<Startup> logger, IApplicationLifetime applicationLifetime,
            IOptionsMonitor<TestCosmosSQLConfiguration> monitor)
        {
            monitor.OnChange(
                vals =>
                {
                    logger.LogInformation($"Configuration updated");
                });


            logger.LogInformation("On Configure Method: TestCosmosSQL");

            app.UseCorrelationId(new CorrelationIdOptions
            {
                Header = "X-Correlation-ID",
                UseGuidForCorrelationId = true,
                UpdateTraceIdentifier = false
            });

            app.UseCAPProblemDetails();

            app.UseMvc();

            if (_environment.IsDevelopment())
            {
                // Use HealthChecks-UI.
                app.UseHealthChecks("/health/live", Configuration["ManagementPort"] ?? "5001", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                app.UseHealthChecksUI(setup =>
                {
                    setup.UIPath = "/health-ui";

                });

                //Swagger Setup for Development Environment
                app.UseSwagger();
                app.UseSwaggerUi3();
            }
            else
            {
                //Basic Health Checks
                app.UseHealthChecks("/health/ready");
                app.UseHealthChecks("/health/live", new HealthCheckOptions()
                {
                    Predicate = (_) => false
                });
            }

        }
        /// <summary>
        /// Maps the custom exceptions(Internal error 500) to a known http error status code like 404,401 etc.
        /// Replace the commented code with your custom exceptions if any.
        /// </summary>
        /// <returns>list of exceptions and mapped http status code.</returns>
        private static Action<ProblemDetailsOptions> MapCustomExceptionsToKnownHttpStatusCode()
        {
            return null;

            //return option => { option.Map<InvalidArgumentException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status400BadRequest)); };

        }
    }
}
