using TestCosmosSQL.Application.Configuration;
using TestCosmosSQL.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using System;

namespace TestCosmosSQL.RestApi.Tests
{
    public class TestCosmosSQLFixture : IDisposable
    {
        // public ITestCosmosSQLService TestCosmosSQLService { get; private set; }
        // public TestCosmosSQLConfiguration TestCosmosSQLConfiguration { get; private set; }
        public ServiceProvider ServiceProviderDi { get; private set; }

        public TestCosmosSQLFixture()
        {
            //   TestCosmosSQLService = new TestCosmosSQLService(null);
            //  TestCosmosSQLConfiguration = new TestCosmosSQLConfiguration();
            var mockLogger = new Mock<ILogger<TestCosmosSQLService>>();
            mockLogger.Setup(logger => logger.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>())
                ).Callback(() => { });

            var _logger = mockLogger.Object;
            var mockEnvironment = new Mock<IHostingEnvironment>();
            mockEnvironment
                    .Setup(m => m.EnvironmentName)
                    .Returns("Development");

            var services = new ServiceCollection();
            services.AddSingleton<ILogger<TestCosmosSQLService>>(_logger);
            services.AddSingleton<ITestCosmosSQLService, TestCosmosSQLService>();
            services.AddSingleton<IHostingEnvironment>(mockEnvironment.Object);


            // Add our Config object so it can be injected
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = configurationBuilder.Build();

            services.Configure<TestCosmosSQLConfiguration>(configuration.GetSection("TestCosmosSQLConfiguration"));



            ServiceProviderDi = services.BuildServiceProvider();

        }
        public void Dispose()
        {
            // Do clean up work here if any
        }
    }
}
