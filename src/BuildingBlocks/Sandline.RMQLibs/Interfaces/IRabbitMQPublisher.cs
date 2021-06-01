using RabbitMQ.Client;
using Sandline.RMQLibs.Entities;

namespace Sandline.RMQLibs.Interfaces
{
    /// <summary>
    /// Sample publisher public interface. You can customize your own interface for the requirement..
    /// </summary>
    public interface IRabbitMQPublisher
    {
        public IModel ConnectMQ();
        public void Publish(IModel channel, Event rmq_event, string rmq_routing_key);
    }
}
