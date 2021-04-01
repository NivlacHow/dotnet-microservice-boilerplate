using AutoMapper;
using EventBus.Messages.Events;
using FileContextExtractor.Entities;
using FileContextExtractor.Helper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FileContextExtractor.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ExtractContextController : ControllerBase
    {
        private readonly IFileProcessorRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public ExtractContextController(IFileProcessorRepository repository, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        [HttpPost("processFiles")]
        public async Task<ActionResult> GetFiles([FromBody]InputPayload model)
        {
            //var files = await _repository.GetFiles(model.FileLocation, model.TargetFileExt);

            return Ok();
        }

    }
}
