using TestCosmosSQL.Application.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace TestCosmosSQL.RestApi.Tests.Applicaion
{
    [Trait("TestCosmosSQLService", "Configuration")]
    public class TestCosmosSQLConfigurationTests : IClassFixture<TestCosmosSQLFixture>
    {
        private readonly TestCosmosSQLConfiguration _configuration;

        public TestCosmosSQLConfigurationTests(TestCosmosSQLFixture fixture)
        {
            _configuration = fixture.ServiceProviderDi.GetService<IOptionsMonitor<TestCosmosSQLConfiguration>>().CurrentValue;
        }


        [Fact]
        public void TestCosmosSQLConfiguration_WhenObjectIsValid_ValidatesAsTrue()
        {
            // Arrange
            // Here you arrange your object(s) for testing valid scenario, this case fixture takecare of it.


            // Act
            // Call Validate method
            _configuration.Validate();

            // Assert 

        }
    }
}
