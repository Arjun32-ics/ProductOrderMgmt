using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderMgmt.Services.ServiceBus
{
    public class ServiceBusTopicTrigger : IServiceBusTopicTrigger
    {
        private readonly ITopicClient _topicClient;
        public ServiceBusTopicTrigger(string connectionstring, string topicName)
        {
            _topicClient = new TopicClient(connectionstring, topicName);
        }
        public async Task SendMessageToServiceTopic(Message message)
        {
            await _topicClient.SendAsync(message);
        }
    }
}
