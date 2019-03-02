using CosmoLibrary.Common;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using ValidationLibrary.Attributes;

namespace CAP.ProblemDetails.Exceptions
{
    public static class ProblemDetailsExtensions
    {
        //If environment is Dev show detailed errors.
        public static bool IsDevelopment { get; internal set; }
        /// <summary>
        /// Add Cap exception, internally it uses  Hellang Nuget.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="isDevelopment"></param>
        /// <returns></returns>
        public static IServiceCollection AddCapProblemDetails(this IServiceCollection services, Action<ProblemDetailsOptions> options,bool isDevelopment)
        {
            IsDevelopment = isDevelopment;
            //If action is sent form startup.cs, include it in the exceptions list.
            if(options != null)
                services.AddProblemDetails(options);

            //Add other known exception and know http status code.
            services.AddProblemDetails(ConfigureProblemDetails);
            
            return services;
        }
        /// <summary>
        /// Use Cap problem detail. Internally it uses Hellang nuget.
        /// </summary>
        /// <param name="app"></param>
        public static void UseCAPProblemDetails(this IApplicationBuilder app)
        {
            //use hellag problem detail.
            app.UseProblemDetails();

        }
        /// <summary>
        /// Action method to map different exception to a known http status code
        /// </summary>
        /// <param name="options"></param>
        private static void ConfigureProblemDetails(ProblemDetailsOptions options) 
        {
            
            // This is the default behavior; only include exception details in a development environment.
            options.IncludeExceptionDetails = ctx => IsDevelopment;
          
            // This will map NotImplementedException to the 501 Not Implemented status code.
            options.Map<NotImplementedException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status501NotImplemented));

            // This will map HttpRequestException to the 503 Service Unavailable status code.
            options.Map<HttpRequestException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status503ServiceUnavailable));


            //All known CAP Exceptions added here.
            // This will map ValidationFailure to the 400 Bad Request status code.
            options.Map<ValidationFailure>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status400BadRequest));

            // This will map ResourceNotFoundException to the 404 Not Found status code.
            options.Map<ResourceNotFoundException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status404NotFound));


            // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
            // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
            options.Map<System.Exception>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));

        }

    }
   


}