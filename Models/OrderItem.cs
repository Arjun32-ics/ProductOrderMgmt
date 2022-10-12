using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductOrderMgmt.Models
{
    [Table("orderitem")]
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
