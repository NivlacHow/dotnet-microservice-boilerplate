using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using Serilog;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using SampleFrontendSvc.Repositories;
using SampleFrontendSvc.Entities;

namespace SampleFrontendSvc.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SampleFrontendSvcController : ControllerBase
    {
        private readonly ISampleRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        private ILogger _logger;
        private readonly IConfiguration _config;

        public SampleFrontendSvcController(ISampleRepository repository, IPublishEndpoint publishEndpoint, /*IMapper mapper,*/ ILogger logger, IConfiguration config)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _logger = logger;
            _config = config;
        }

        [HttpPost("SampleMethod")]
        public async Task<ActionResult> SampleInnerMethod([FromBody] InputPayload input)
        {
            try
            {
                _logger.Information("[SampleInnerMethod] - Param1:[" + input.Param1 + "] - Param2:[" + input.Param2 + "]");
                string get_data = await _repository.Get(input.Param1);
                string save_data = await _repository.Save(input.Param2);

                return Ok("Calling sample method finished.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest();
            }
        }
    }
}
