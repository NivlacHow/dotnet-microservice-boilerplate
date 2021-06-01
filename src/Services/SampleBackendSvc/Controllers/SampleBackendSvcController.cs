using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SampleBackendSvc.Entities;
using System;

namespace SampleBackendSvc.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SampleBackendSvcController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public SampleBackendSvcController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }
        
        [HttpPost("testAPI")]
        public ActionResult testAPI([FromBody]InputPayload model)
        {
            return Ok();
        }

    }
}
