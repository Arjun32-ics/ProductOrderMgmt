using AutoMapper;
using ProductOrderMgmt.DTOs;
using ProductOrderMgmt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductOrderMgmt.Mapper
{
     class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
        }
    }       
}
