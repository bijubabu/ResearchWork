using TestCosmosSQL.Application.Services;
using System;
using System.Threading;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace TestCosmosSQL.RestApi.Tests.Applicaion
{
    [Trait("TestCosmosSQLService", "Services")]
    public class TestCosmosSQLServiceTests : IClassFixture<TestCosmosSQLFixture>
    {
        private readonly ITestCosmosSQLService _service;
        public TestCosmosSQLServiceTests(TestCosmosSQLFixture fixture)
        {
            _service = fixture.ServiceProviderDi.GetService<ITestCosmosSQLService>();
        }


        [Fact]
        public void GetResourceAsync_WhenCancellationRequested_ThrowsOperationCanceledException()
        {
            // Arrange
            // Here you arrange your object(s) for testing valid scenario
            var cancel = new CancellationTokenSource();

            // Act
            // Assert
            cancel.Cancel(true);
            Assert.Throws<OperationCanceledException>(() => _service.GetResourceAsync(Guid.NewGuid(), cancel.Token).Result);
        }
    }
}
