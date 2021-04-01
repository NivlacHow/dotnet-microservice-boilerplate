using FileContextExtractor.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileContextExtractor.Queues
{
    public class EventBusRabbitMQ : IDisposable
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private IModel _consumerChannel;
        private string _queueName;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        public EventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, ILogger<EventBusRabbitMQ> logger,
            string queueName = null)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger;
            _queueName = queueName;
        }
        public IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            //Create event when something receive
            consumer.Received += ReceivedEvent;

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };
            return channel;
        }

        private void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == "demo-files-queue")
            {
                //Let's do our processing here with the files......
                var body = Encoding.UTF8.GetString(e.Body.ToArray());
                var values = JsonConvert.DeserializeObject<Root>(body);
                _logger.LogInformation("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
                _logger.LogInformation("JSON message: " + body);
                _logger.LogInformation("Event Received From Service: " + values.Name.ToString());
                _logger.LogInformation("Working on Message :"+ values.Message.Key.ToString());
                _logger.LogInformation("Working on File :"+ values.Message.Value.FileName.ToString());
                Thread.Sleep(30000);
                _logger.LogInformation("File processed: " + values.Message.Value.FileName.ToString()); ;
                _logger.LogInformation("Fetching another file from Queue....");
                _logger.LogInformation("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
                ////
                //Publish another event here for next Queue to process.....
                //PublishFeedback("SomeOtherQueue", e.BasicProperties.Headers);
            }

            if (e.RoutingKey == "demo-SomeOther-queue")
            {
                //Implementation here
            }
        }

        public void PublishFeedback(string _queueName, IDictionary<string, object> headers)
        {

            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {

                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject("Some input Payload");
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;
                properties.Headers = headers;
                // properties.Expiration = "36000000";
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
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
        }

    }
}
