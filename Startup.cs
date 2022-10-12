using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductOrderMgmt.Data;
using ProductOrderMgmt.Mapper;
using ProductOrderMgmt.Services;
using ProductOrderMgmt.Services.Order;
using ProductOrderMgmt.Services.ServiceBus;
using ProductOrderMgmt.Services.Storage;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(ProductOrderMgmt.Startup))]
namespace ProductOrderMgmt
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            builder.Services.AddDbContext<OrderMgtDbContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable("AzureDbConnection")));

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();


            builder.Services.AddSingleton<ICloudStorageService>(serviceProvider =>
            new CloudStorageService(Environment.GetEnvironmentVariable("StorageAccountAzure")));

            builder.Services.AddSingleton<IServiceBusQueueService>(serviceProvider =>
            new ServiceBusQueueService(Environment.GetEnvironmentVariable("ServiceBusConnectionString"),"orders"));

            builder.Services.AddSingleton<IServiceBusTopicTrigger>(serviceProvider =>
           new ServiceBusTopicTrigger(Environment.GetEnvironmentVariable("ServiceBusConnectionString"), "orders"));

        }
    }
}