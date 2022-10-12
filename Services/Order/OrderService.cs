using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductOrderMgmt.Data;
using ProductOrderMgmt.DTOs;
using ProductOrderMgmt.Enums;
using ProductOrderMgmt.Models;
using ProductOrderMgmt.Services.ServiceBus;
using ProductOrderMgmt.Services.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderMgmt.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly ICloudStorageService _cloudStorageService;
        private static string storageQueueName = "orders";
        private readonly IMapper _mapper;
        private readonly OrderMgtDbContext _orderMgtDbContext;
        private readonly IServiceBusQueueService _serviceBusQueueService;
        private readonly IServiceBusTopicTrigger _serviceBusTopicTrigger;

        public OrderService(ICloudStorageService cloudStorageService, IMapper mapper,
            OrderMgtDbContext orderMgtDbContext, IServiceBusQueueService serviceBusQueueService,
            IServiceBusTopicTrigger serviceBusTopicTrigger)
        {
            _cloudStorageService = cloudStorageService;
            _mapper = mapper;
            _orderMgtDbContext = orderMgtDbContext;
            _serviceBusQueueService = serviceBusQueueService;
            _serviceBusTopicTrigger = serviceBusTopicTrigger;
        }
        public async Task SendMessageToQueueStorage(OrderDTO data)
        {
            data.OrderId = Guid.NewGuid();
            data.OrderNumber = Guid.NewGuid().ToString("N");
            var serializedData = JsonConvert.SerializeObject(data);
            await _cloudStorageService.SendMessageToQueue(storageQueueName, serializedData);
        }

        public async Task CreateOrder(OrderDTO orderDTO)
        {

            ProductOrderMgmt.Models.Order order = _mapper.Map<OrderDTO, ProductOrderMgmt.Models.Order>(orderDTO);
            order.CreatedBy = Guid.NewGuid();
            order.CreatedDate = DateTime.UtcNow;
            order.Status = OrderStatus.I.ToString();

            foreach (OrderItem item in order.OrderItems) 
            {
                item.OrderId = order.OrderId;
                item.OrderItemId = Guid.NewGuid();
                item.CreatedBy = Guid.NewGuid();
                item.CreatedDate = DateTime.UtcNow;
            }
            await _orderMgtDbContext.Orders.AddAsync(order);
            await _orderMgtDbContext.SaveChangesAsync();
        }

        public async Task ArchiveOrder()
        {
            IEnumerable<ProductOrderMgmt.Models.Order> deliveredOrder = await _orderMgtDbContext.Orders.Where(
                x => x.Status == OrderStatus.D.ToString() && !x.IsArchieved).ToListAsync();
                

            foreach (ProductOrderMgmt.Models.Order order in deliveredOrder)
            {
                order.IsArchieved = true;
            }

            _orderMgtDbContext.Orders.UpdateRange(deliveredOrder);
            await _orderMgtDbContext.SaveChangesAsync();
        }

        public async Task SendMessageToServiceBusQueue(OrderDTO data)
        {
            data.OrderId = Guid.NewGuid();
            data.OrderNumber = Guid.NewGuid().ToString("N");
            var serializeddata = JsonConvert.SerializeObject(data);
            await _serviceBusQueueService.SendMessageToServiceQueue(new Microsoft.Azure.ServiceBus.Message
            {
                Body = Encoding.UTF8.GetBytes(serializeddata),
                CorrelationId = Guid.NewGuid().ToString(),
                Label = "createorder"
            });
        }
        public async Task SendMessageToServiceBusTopic(OrderDTO data)
        {
            data.OrderId = Guid.NewGuid();
            data.OrderNumber = Guid.NewGuid().ToString("N");
            var serializeddata = JsonConvert.SerializeObject(data);
            await _serviceBusTopicTrigger.SendMessageToServiceTopic(new Microsoft.Azure.ServiceBus.Message
            {
                Body = Encoding.UTF8.GetBytes(serializeddata),
                CorrelationId = Guid.NewGuid().ToString(),
                Label = "createorder"
            });
        }
    }
}
