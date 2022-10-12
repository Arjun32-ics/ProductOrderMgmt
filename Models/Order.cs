using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductOrderMgmt.Models
{
    [Table("order")]
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsArchieved { get; set; }
        
        public IEnumerable<OrderItem> OrderItems { get; set; }

    }
}
