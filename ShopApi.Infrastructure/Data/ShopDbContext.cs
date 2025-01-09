using Microsoft.EntityFrameworkCore;
using ShopApi.Core.Entities;

namespace ShopApi.Infrastructure.Data
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.DateOfBirth)
                    .IsRequired();
                entity.Property(e => e.RegistrationDate)
                    .IsRequired();
                entity.HasMany(e => e.Purchases)
                    .WithOne(p => p.Customer)
                    .HasForeignKey(p => p.CustomerId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.SKU)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasPrecision(10, 2);
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Date)
                    .IsRequired();
                entity.Property(e => e.TotalCost)
                    .IsRequired()
                    .HasPrecision(10, 2);
                entity.HasMany(e => e.PurchaseItems)
                    .WithOne(pi => pi.Purchase)
                    .HasForeignKey(pi => pi.PurchaseId);
            });

            modelBuilder.Entity<PurchaseItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity)
                    .IsRequired();
                entity.HasOne(pi => pi.Product)
                    .WithMany()
                    .HasForeignKey(pi => pi.ProductId);
            });
        }
    }
}