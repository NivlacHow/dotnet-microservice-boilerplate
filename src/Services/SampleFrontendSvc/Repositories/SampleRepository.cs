using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Sandline.RMQLibs.Entities;

namespace SampleFrontendSvc.Repositories
{
    public class SampleRepository : ISampleRepository
    {
        private readonly IDistributedCache _redisCache;
        private IOptions<EventBusSettings> _eventBusConfig;

        public SampleRepository(IDistributedCache cache, IOptions<EventBusSettings> eventBusConfig)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
            _eventBusConfig = eventBusConfig;
        }

        public async Task<string> Get(string data)
        {
            return "Get data from repository:" + data;
        }

        public async Task<string> Save(string data)
        {
            var smplEvent = new Event
            {
                Publisher = "Sample-frontend-controller",
                Message = "This is a sample call",
                Data = "this could be an object"
            };
            SendMsgToRMQ(smplEvent, "smpl-routing-key");
            return "Data:" + data + "Saved.";
        }

        public void SendMsgToRMQ(Event rmq_event, string rmq_routing_key)
        {
            var RmqPublisher = new Sandline.RMQLibs.Services.RabbitMQPublisher(_eventBusConfig);
            var RmqChannel = RmqPublisher.ConnectMQ();
            RmqPublisher.Publish(RmqChannel, rmq_event, rmq_routing_key);
        }
    }
}
