using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestCosmosSQL.Application.Services;
using System.Threading;
using CosmoLibrary.Common;
using TestCosmosSQL.Application.DataTransferObjects;

using Microsoft.Extensions.Logging;
using TestCosmosSQL.Application.Configuration;
using Microsoft.Extensions.Options;
using TestCosmosSQL.Domain.Models;

namespace TestCosmosSQL.RestApi.Controllers
{
    /// <summary>
    /// rest API Controller
    /// </summary>
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/[controller]")]
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
        /// Interface to communicate with repository
        /// </summary>
        private readonly IDocumentDbRepository<PlaceModel> _documentRepository;

        /// <summary>
        /// Constructor, use dependency injection to inject custom services
        /// </summary>
        /// <param name="service">TestCosmosSQL service</param>
        /// <param name="logger">Loggin service</param>
        /// <param name="configuration">Loggin service</param>
        public PlacesController(ITestCosmosSQLService service, IDocumentDbRepository<PlaceModel> documentDbRepository,
            ILogger<PlacesController> logger, IOptionsMonitor<TestCosmosSQLConfiguration> configuration)
        {
            _service = service;
            _documentRepository = documentDbRepository;
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
        public async Task<ActionResult<PlaceCreateDto>> Get([Required] Guid id)
        {
            return await _service.GetResourceAsync(id, CancellationToken.None);
        }

        [HttpGet]
        public async Task<ActionResult<List<PlaceGetDto>>> GetAsync()
        {
            var placesDto = new List<PlaceGetDto>();
            try
            {
                var places = await _documentRepository.GetAsync();

                // Map PlaceModel to PlaceDto
                // Map the Dto with model
                foreach (var place in places)
                {
                    var placeDto = new PlaceGetDto()
                    {
                        Id = place.Id,
                        Type = place.Type,
                        Name = place.Name,
                        Latitude = place.Latitude,
                        Longitude = place.Longitude,
                        Status = place.Status
                    };

                    placesDto.Add(placeDto);
                }

                return placesDto;
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PlaceGetDto>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Id is Required");
            try
            {
                var place = await _documentRepository.GetAsync(id);

                // Map PlaceModel to PlaceDto
                // Map the Dto with model
                var placeDto = new PlaceGetDto()
                {
                    Id = place.Id,
                    Type = place.Type,
                    Name = place.Name,
                    Latitude = place.Latitude,
                    Longitude = place.Longitude,
                    Status = place.Status
                };

                return placeDto;
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] PlaceCreateDto placeDto)
        {
            if (placeDto == null) return BadRequest("Place information is Required");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Map the Dto with model
            var placeModel = new PlaceModel()
            {
                Id = Guid.NewGuid(),
                Type = "Place",
                Name = placeDto.Name,
                Latitude = placeDto.Latitude,
                Longitude = placeDto.Longitude,
                Status = placeDto.Status
            };

            await _documentRepository.UpsertAsync(placeModel);

            return CreatedAtAction(nameof(GetByIdAsync), new { placeModel.Id }, placeModel);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Id is Required");

            if (_documentRepository.ExistAsync(id).Result)
            {
                await _documentRepository.DeleteAsync(id);
            }

            return Ok();
        }
    }
}
