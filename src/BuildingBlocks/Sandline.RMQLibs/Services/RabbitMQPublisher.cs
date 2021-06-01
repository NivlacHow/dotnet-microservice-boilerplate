using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Sandline.RMQLibs.Entities;
using Sandline.RMQLibs.Interfaces;
using System;
using System.Text;

namespace Sandline.RMQLibs.Services
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private EventBusSettings _appConfig;


        public RabbitMQPublisher(IOptions<EventBusSettings> appConfig)
        {
            _appConfig = appConfig.Value;
        }
        public IModel ConnectMQ()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_appConfig.HostAddress)
            };
            IConnection rmqConnection = factory.CreateConnection();
            return rmqConnection.CreateModel();


            //Publish(channel, data);
        }
        public void Publish(IModel channel, Event rmq_event, string rmq_routing_key)
        {
            // Use Routing-key can publish messages to multiple Queues.
            // channel.QueueDeclare(queue: rmq_queue_name, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var msg_body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rmq_event));
            channel.BasicPublish(exchange: "", routingKey: rmq_routing_key, null, body: msg_body);
        }
    }
}
