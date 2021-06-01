using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Sandline.RMQLibs.Interfaces;

namespace Sandline.RMQLibs.Services
{
    public class RabbitMQEventBus : IDisposable
    {
        private readonly IRabbitMQAdapter _rmqAdapter;
        private IModel _rmqConsumer;
        private string _monitorQueue;
        private readonly ILogger<RabbitMQEventBus> _logger;


        public RabbitMQEventBus(IRabbitMQAdapter persistent_adapter, ILogger<RabbitMQEventBus> logger,
            string monitorQueueName = null)
        {
            _rmqAdapter = persistent_adapter ?? throw new ArgumentNullException(nameof(persistent_adapter));
            _logger = logger;
            _monitorQueue = monitorQueueName;
        }
        public IModel CreateConsumer(IModel rmqModel)
        {
            if (!_rmqAdapter.IsConnected)
            {
                _rmqAdapter.TryConnect();
            }

            var consumer = new EventingBasicConsumer(rmqModel);
            consumer.Received += ReceivedEvent;

            rmqModel.BasicConsume(queue: _monitorQueue, autoAck: true, consumer: consumer);
            rmqModel.CallbackException += (sender, ea) =>
            {
                _rmqConsumer.Dispose();
                _rmqConsumer = CreateConsumer(rmqModel);
            };
            return rmqModel;
        }

        private void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            // According to the RMQ-Publisher, customize the RoutingKey.
            if (e.RoutingKey == "sample-routing-key")
            {
                var body = Encoding.UTF8.GetString(e.Body.ToArray());
                var values = JsonConvert.DeserializeObject<Entities.Event>(body);
                _logger.LogInformation("Received a new event: " + body);
                Thread.Sleep(30000);

                //Publish another event here for next Queue to process.....
                //PublishFeedback("SomeOtherQueue", e.BasicProperties.Headers);
            }

            // To add more receive logic here if we need.
            // if (e.RoutingKey == "sample-routing-key2"){}
        }

        public void PublishFeedback(string _queueName, IDictionary<string, object> headers)
        {

            if (!_rmqAdapter.IsConnected)
            {
                _rmqAdapter.TryConnect();
            }

            using (var channel = _rmqAdapter.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject("Some input Payload");
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;
                properties.Headers = headers;
                //properties.Expiration = "36000000";
                //properties.ContentType = "text/plain";

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "", routingKey: _queueName, mandatory: true, basicProperties: properties, body: body);
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("Sent RabbitMQ");
                    //implement ack handle
                };
                channel.ConfirmSelect();
            }
        }

        public void Dispose()
        {
            if (_rmqConsumer != null)
            {
                _rmqConsumer.Dispose();
            }
        }

    }
}
