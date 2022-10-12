using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProductOrderMgmt.Data;
using ProductOrderMgmt.DTOs;
using ProductOrderMgmt.Models;
using ProductOrderMgmt.Services.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderMgmt.Services
{
    public class ProductService : IProductService
    {
        private readonly OrderMgtDbContext _orderMgtDbContext;
        private readonly IMapper _mapper;
        private readonly ICloudStorageService _cloudStorageService;
        private static string productContainerName = "products";

        public ProductService(OrderMgtDbContext orderMgtDbContext, IMapper mapper, ICloudStorageService cloudStorageService)
        {
            _orderMgtDbContext = orderMgtDbContext;
           _mapper = mapper;
            _cloudStorageService = cloudStorageService;
        }
       
        public async Task<Guid> CreateProduct(ProductDto productdto)
        {
           
                Product product = _mapper.Map<ProductDto, Product>(productdto);
                product.ProductId = Guid.NewGuid();
                product.CreatedBy = Guid.NewGuid();
                product.CreatedDate = DateTime.UtcNow;
                product.UpdatedBy = Guid.NewGuid();
               product.Updateddate = DateTime.UtcNow;
               await _orderMgtDbContext.Product.AddAsync(product);
                await _orderMgtDbContext.SaveChangesAsync();
            return product.ProductId;

            


        }

        public async Task UploadProductImage(IFormFileCollection files, Guid productId)
        {
            await _cloudStorageService.UploadFileToBlob(files, productId, productContainerName);
        }
    }
}
