using Microsoft.EntityFrameworkCore;
using ProductOrderMgmt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductOrderMgmt.Data
{
   public class OrderMgtDbContext : DbContext
    {
        public OrderMgtDbContext(DbContextOptions<OrderMgtDbContext> options) : base(options)
        { }

        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

    }
}
