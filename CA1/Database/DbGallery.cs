using CA1.Models;
using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CA1.Database
{
    public class DbGallery : DbContext
    {
        protected IConfiguration configuration;

        public DbGallery(DbContextOptions<DbGallery> options)
            : base (options)
        {

        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            //setting composite PK for OrderDetail table
            //model.Entity<OrderDetail>().HasKey(x => new { x.OrderId, x.ProductId });
            //setting composite key for ShoppingCart table
            //model.Entity<ShoppingCartDetail>().HasAlternateKey(x => new { x.UserId, x.ProductId });
            //.HasKey creates a composite primary key
            //.HasAlternateKey creates a composite alternate key
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ShoppingCartDetail> ShoppingCart { get; set; }
        public DbSet<Session> Sessions { get; set; }
    }
}
