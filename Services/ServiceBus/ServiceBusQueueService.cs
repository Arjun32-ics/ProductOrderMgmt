using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderMgmt.Services.ServiceBus
{
    public class ServiceBusQueueService : IServiceBusQueueService
    {
        private readonly IQueueClient _queueClient;
        public ServiceBusQueueService(string connectionstring , string queueName)
        {
            _queueClient = new QueueClient(connectionstring, queueName);
        }
        public async Task SendMessageToServiceQueue(Message message)
        {
            await _queueClient.SendAsync(message);
        }
    }
}
