using ProductOrderMgmt.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderMgmt.Services.Order
{
   public interface IOrderService
    {
        Task SendMessageToQueueStorage(OrderDTO data);
        Task CreateOrder(OrderDTO orderDTO);
        Task ArchiveOrder();
        Task SendMessageToServiceBusQueue(OrderDTO data);
        Task SendMessageToServiceBusTopic(OrderDTO data);



    }
}
