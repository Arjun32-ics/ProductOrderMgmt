using System;
using System.Collections.Generic;
using System.Text;

namespace ProductOrderMgmt.Models
{
   public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? Updateddate { get; set; }





    }
}
