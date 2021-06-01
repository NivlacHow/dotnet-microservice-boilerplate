using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Threading;
using Sandline.RMQLibs.Interfaces;

namespace Sandline.RMQLibs.Services
{
    public class RabbitMQAdapter : IRabbitMQAdapter
    {
        private readonly IConnectionFactory _rmqConnectionFactory;
        RabbitMQEventBus _rmqEventBus;
        IModel _rmqModel;
        IConnection _rmqConnection;
        bool _disposed;
        ILogger<RabbitMQEventBus> _logger;


        /// <param name="connectionFactory">RMQ</param>
        /// <param name="logger"></param>
        public RabbitMQAdapter(IConnectionFactory connectionFactory, ILogger<RabbitMQEventBus> logger)
        {
            _logger = logger;
            _rmqConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            if (!IsConnected)
            {
                TryConnect();
            }
        }

        /// <summary>
        /// Create a new Model, EventBus, Queue and Consumer for this RMQ-Adapter.
        /// </summary>
        public void Initialize(string monitorQueue)
        {
            if (!IsConnected)
            {
                TryConnect();
            }

            //Create a new channel
            _rmqEventBus = new RabbitMQEventBus(this, _logger, monitorQueueName: monitorQueue);
            _rmqModel = CreateModel();
            _rmqModel.QueueDeclare(queue: monitorQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _rmqEventBus.CreateConsumer(_rmqModel);

        }


        public bool TryConnect()
        {
            try
            {
                _logger.LogInformation("RabbitMQ Client is trying to connect...");
                _rmqConnection = _rmqConnectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException e)
            {
                _logger.LogError("RabbitMQ Client connecting error:" + e.Message);
                Thread.Sleep(5000);
                _logger.LogInformation("RabbitMQ Client is trying to reconnect...");
                _rmqConnection = _rmqConnectionFactory.CreateConnection();
            }

            if (IsConnected)
            {
                _rmqConnection.ConnectionShutdown += OnConnectionShutdown;
                _rmqConnection.CallbackException += OnCallbackException;
                _rmqConnection.ConnectionBlocked += OnConnectionBlocked;

                _logger.LogInformation($"RabbitMQ persistent connection acquired a connection {_rmqConnection.Endpoint.HostName} and is subscribed to failure events");

                return true;
            }
            else
            {
                //  implement send warning email here
                //-----------------------
                _logger.LogError("FATAL ERROR: RabbitMQ connections could not be created and opened.");
                return false;
            }

        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }
            return _rmqConnection.CreateModel();
        }


        public void Disconnect()
        {
            if (_disposed)
            {
                return;
            }
            Dispose();
        }

        public bool IsConnected
        {
            get
            {
                return _rmqConnection != null && _rmqConnection.IsOpen && !_disposed;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _rmqConnection.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogError("Dispose error:" + ex.Message);
            }
        }


        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            _logger.LogInformation("A RabbitMQ connection is shutdown. Trying to re-connect...");
            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            _logger.LogError("A RabbitMQ connection throw exception. Trying to re-connect...");
            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;
            _logger.LogInformation("A RabbitMQ connection is on shutdown. Trying to re-connect...");
            TryConnect();
        }

    }
}
