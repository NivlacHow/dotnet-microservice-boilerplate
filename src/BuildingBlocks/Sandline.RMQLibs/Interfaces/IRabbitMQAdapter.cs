using RabbitMQ.Client;

namespace Sandline.RMQLibs.Interfaces
{
    public interface IRabbitMQAdapter
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();

        void Initialize(string channelName);

        void Disconnect();
    }
}
