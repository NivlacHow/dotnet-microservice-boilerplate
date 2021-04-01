using FileProcessor.Entities;
using FileProcessor.Helper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileProcessor.Queues
{
    public class Publisher : IPublisher
    {
        private EventBusSettings _appConfig;
        public Publisher(IOptions<EventBusSettings> appConfig)
        {
            _appConfig = appConfig.Value;
        }
        public void ConnectMQ(IDictionary<string, FileModel> data)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_appConfig.HostAddress)
            };
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();
            Publish(channel, data);
        }
        public void Publish(IModel channel,IDictionary<string, FileModel> data)
        {
            //channel.QueueDeclare("demo-files-queue",
            //    durable: false,
            //    exclusive: false,
            //    autoDelete: false,
            //    arguments: null);
           

            foreach (var file in data)
            {
                var message = new { Name = "File Processor Service", Message = file };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish("", "demo-files-queue", null, body);
            }
        }
    }
}
