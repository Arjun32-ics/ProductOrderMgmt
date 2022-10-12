using System;
using System.Collections.Generic;
using System.Text;

namespace ProductOrderMgmt.DTOs
{
    public class OrderItemDTO
    {
        public Guid ProductId { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
    }
}
