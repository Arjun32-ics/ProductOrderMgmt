using System;
using System.Collections.Generic;
using System.Text;

namespace ProductOrderMgmt.DTOs
{
   public class OrderDTO
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public Guid CustomerId { get; set; }
        public IEnumerable<OrderItemDTO> OrderItems { get; set; }

    }
}
