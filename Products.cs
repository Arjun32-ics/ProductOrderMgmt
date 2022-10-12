using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductOrderMgmt.Services;
using ProductOrderMgmt.DTOs;
using ProductOrderMgmt.Models;

namespace ProductOrderMgmt
{
    public  class Products
    {
        private readonly IProductService _productService;

        public Products(IProductService productService)
        {
            _productService = productService;
        }

        [FunctionName("CreateProduct")]
        public  async Task<IActionResult> CreateProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ProductDto data = JsonConvert.DeserializeObject<ProductDto>(requestBody);
           // Product data = JsonConvert.DeserializeObject<Product>(requestBody);
            Guid pId= await _productService.CreateProduct(data);

            return new CreatedResult(string.Empty, pId);
        }

        [FunctionName("UploadProductImage")]
        public async Task<IActionResult> UploadProductImage(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products/{productId}/upload")] HttpRequest req,
           ILogger log , Guid productId)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            IFormFileCollection formFiles = req.Form.Files;
            await _productService.UploadProductImage(formFiles, productId);
            return new OkResult();
        }

        [FunctionName("ProductBlobCreated")]
        public void ProductBlobCreated([BlobTrigger("products/{name}", Connection = "StorageAccountAzure")] Stream myBlob, string name, ILogger log)
        {
            //do something
            string[] blobName = name.Split('/');
            log.LogInformation($"Product Blob Created Successfully for product id {blobName[0]} with name {blobName[1]}");
        }
    }
}
