using AutoMapper;
using EventBus.Messages.Events;
using FileProcessor.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FileProcessor.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FileProcessorController : ControllerBase
    {
        private readonly IFileProcessorRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private ILogger _logger;
        private readonly IConfiguration _config;

        public FileProcessorController(IFileProcessorRepository repository, IPublishEndpoint publishEndpoint, IMapper mapper,ILogger logger, IConfiguration  config)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
            _config = config;
        }

        
        [HttpPost("processFiles")]
        public async Task<ActionResult> GetFiles([FromBody]InputPayload model)
        {
            try
            {
                _logger.Information("Request Received to get some files");
                _logger.Information("Paramters are: [" + model.FileLocation + "] & [" + model.TargetFileExt + "]");
                var files = await _repository.GetFiles(model.FileLocation, model.TargetFileExt);

                return Ok(files);
            }catch(Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest();
            }
        }   
        [HttpGet("getSecretValues")]
        public ActionResult getSecretValues()
        {
            try
            {
                var jsonSettings = JsonConvert.SerializeObject(_config["ListOfDatabases:SQL1"]);
                var jsonSettings2 = JsonConvert.SerializeObject(_config["ListOfDatabases:SQL2"]);
                var jsonSettingsMain = _config["ListOfDatabases"];
                return Ok(new
                {
                    MainTree = jsonSettingsMain,
                    Database1 = jsonSettings,
                    Database2 = jsonSettings2
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest();
            }
        }
    }
}
