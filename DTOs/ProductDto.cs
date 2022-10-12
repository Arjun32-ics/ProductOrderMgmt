using System;
using System.Collections.Generic;
using System.Text;

namespace ProductOrderMgmt.DTOs
{
   public class ProductDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }


    }
}
