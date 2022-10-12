using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductOrderMgmt.DTOs;
using ProductOrderMgmt.Services.Order;
using Microsoft.Azure.Documents;
using System.Collections.Generic;

namespace ProductOrderMgmt
{
    public  class Orders
    {
        private readonly IOrderService _orderService;
        public Orders(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [FunctionName("CreateOrderToQueue")]
        public  async Task<IActionResult> CreateOrderToQueue(
            [HttpTrigger(AuthorizationLevel.Anonymous,  "post", Route = "orders/{customerId}/queue")] HttpRequest req,
            ILogger log,Guid customerId)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderDTO data = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
            data.CustomerId = customerId;
            await _orderService.SendMessageToQueueStorage(data);
           

            return new AcceptedResult();
        }
        [FunctionName("ProcessOrderToQueue")]
        public async Task  ProcessOrderToQueue([QueueTrigger("orders", Connection = "StorageAccountAzure")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            OrderDTO data = JsonConvert.DeserializeObject<OrderDTO>(myQueueItem);
            await _orderService.CreateOrder(data);
        }

        [FunctionName("ArchiveOrders")]
        public async Task ArchiveOrders([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            await _orderService.ArchiveOrder(); 

        }
        [FunctionName("CreateOrderToServiceBusQueue")]
        public async Task<IActionResult> CreateOrderToServiceBusQueue(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "customers/{customerId}/orders")] HttpRequest req,
           ILogger log, Guid customerId)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderDTO data = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
            data.CustomerId = customerId;
            await _orderService.SendMessageToServiceBusQueue(data);

            return new AcceptedResult();
        }
        [FunctionName("ProcessServiceBusQueueOrder")]
        public async Task ProcessServiceBusQueueOrder([ServiceBusTrigger("orders", Connection = "ServiceBusConnectionString")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            OrderDTO data = JsonConvert.DeserializeObject<OrderDTO>(myQueueItem);
            await _orderService.CreateOrder(data);
        }
        [FunctionName("CreateOrderToServiceBusTopic")]
        public async Task<IActionResult> CreateOrderToServiceBusTopic(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "customers/{customerId}/orderstopic")] HttpRequest req,
         ILogger log, Guid customerId)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderDTO data = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
            data.CustomerId = customerId;
            await _orderService.SendMessageToServiceBusTopic(data);

            return new AcceptedResult();
        }

        [FunctionName("ProcessServiceBusTopic")]
        public async Task ProcessServiceBusTopic([ServiceBusTrigger("orders", "createorder", Connection = "ServiceBusConnectionString")] string mySbMsg, ILogger Log)
        {
            Log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            OrderDTO data = JsonConvert.DeserializeObject<OrderDTO>(mySbMsg);
            await _orderService.CreateOrder(data);
        }
        [FunctionName("CreateOrderToCosmosDb")]
        public async Task<IActionResult> CreateOrderToCosmosDb(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "customers/{customerId}/ordercosmosdb")] HttpRequest req,
        [CosmosDB(databaseName:"ordermanagementsystem",collectionName:"orders",ConnectionStringSetting ="AzureCosmosDbConnectionString")]
        IAsyncCollector<OrderDTO> orders,
        ILogger log, Guid customerId)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderDTO data = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
            data.CustomerId = customerId;
            data.OrderId = Guid.NewGuid();
            data.OrderNumber = Guid.NewGuid().ToString("N");
            await orders.AddAsync(data);
            return new CreatedResult(string.Empty,null);
        }
        [FunctionName("CosmosDbTrigger")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ordermanagementsystem",
            collectionName: "orders",
            ConnectionStringSetting = "AzureCosmosDbConnectionString",
            LeaseCollectionName = "leases",CreateLeaseCollectionIfNotExists =true)]IReadOnlyList<Document> input,
           ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
