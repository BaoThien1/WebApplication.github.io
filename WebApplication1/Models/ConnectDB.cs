namespace WebApplication1.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ConnectDB : DbContext
    {
        public ConnectDB()
            : base("name=ConnectDB")
        {
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartDetail> CartDetails { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .Property(e => e.Total)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.Total);
              


            modelBuilder.Entity<InvoiceDetail>()
                .Property(e => e.UnitPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .Property(e => e.SaleOff)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Invoice>()
           .Property(i => i.InvoiceDate)
           .HasColumnType("datetime2");
        }
    }
}
