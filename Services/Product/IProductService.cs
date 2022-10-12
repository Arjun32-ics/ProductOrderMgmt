using Microsoft.AspNetCore.Http;
using ProductOrderMgmt.DTOs;
using ProductOrderMgmt.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderMgmt.Services
{
   public interface IProductService
    {
        Task<Guid> CreateProduct(ProductDto product);

        Task UploadProductImage(IFormFileCollection files, Guid productId);

    }
}
