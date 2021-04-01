using FileProcessor.Entities;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileProcessor.Queues
{
    interface IPublisher
    {
        public void ConnectMQ(IDictionary<string, FileModel> data);
        public void Publish(IModel channel, IDictionary<string, FileModel> data);
    }
}
