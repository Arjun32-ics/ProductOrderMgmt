using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderMgmt.Services.ServiceBus
{
   public interface IServiceBusTopicTrigger
    {
        Task SendMessageToServiceTopic(Message message)
    }
}
