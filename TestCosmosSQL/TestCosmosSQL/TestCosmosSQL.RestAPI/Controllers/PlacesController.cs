using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestCosmosSQL.Application.Services;
using System.Threading;
using TestCosmosSQL.Application.DataTransferObjects;

using Microsoft.Extensions.Logging;
using TestCosmosSQL.Application.Configuration;
using Microsoft.Extensions.Options;

namespace TestCosmosSQL.RestApi.Controllers
{
    /// <summary>
    /// rest API Controller
    /// </summary>
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class PlacesController : ControllerBase
    {
        /// <summary>
        /// Customer Repository
        /// </summary>
        private readonly ITestCosmosSQLService _service;
        /// <summary>
        /// logger for the controller
        /// </summary>
        private readonly ILogger<PlacesController> _logger;
        /// <summary>
        /// configuration for the controller
        /// </summary>
        private readonly TestCosmosSQLConfiguration _configuration;

        /// <summary>
        /// Constructor, use dependency injection to inject custom services
        /// </summary>
        /// <param name="service">TestCosmosSQL service</param>
        /// <param name="logger">Loggin service</param>
        /// <param name="configuration">Loggin service</param>
        public PlacesController(ITestCosmosSQLService service,
            ILogger<PlacesController> logger,
            IOptionsMonitor<TestCosmosSQLConfiguration> configuration)
        {
            _service = service;
            _logger = logger;
            _configuration = configuration.CurrentValue;
        }

        /// <summary>
        /// Get List of Resources
        /// </summary>
        /// <param name="id" > Resource identifier GUID </param>
        /// <remarks>This API list all the resources for the resource id passed</remarks>
        /// <returns >Resources</returns>

        [HttpGet("{id}")]
        public async Task<ActionResult<TestCosmosSQLDto>> Get([Required] Guid id)
        {
            return await _service.GetResourceAsync(id, CancellationToken.None);
        }
    }
}
