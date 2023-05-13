using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleForTahaBazar.Entities.Models;

namespace SampleForTahaBazar.Entities
{
    public class SaleDbContext : DbContext
    {
        public SaleDbContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Id used as Primary Key By Convention
            // Table name created by Convention 
            // Foreign Key? I am not sure they created in database when it should with the specified name altough the name is conventional here and it should create!
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Invoice>().ToTable("Invoices");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<UnitOfMeasurement>().ToTable("UnitOfMeasurements");
            modelBuilder.Entity<SaleLine>().ToTable("SaleLines");
            modelBuilder.Entity<SaleLine>().HasOne<Invoice>(s => s.Invoice).WithMany(i => i.SaleLines).HasForeignKey("InvoiceId");
            modelBuilder.Entity<SaleLine>().HasOne<Product>(s => s.Product).WithMany(p => p.SaleLines).HasForeignKey("ProductId");
            modelBuilder.Entity<SaleLine>().HasOne<UnitOfMeasurement>(s => s.UnitOfMeasurement).WithMany(u => u.SaleLines).HasForeignKey("UnitOfMeasurementId");

        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.State == EntityState.Added)
                {
                    item.CurrentValues["Created"] = DateTimeOffset.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }


        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<UnitOfMeasurement> UnitOfMeasurements { get; set; }
        public virtual DbSet<SaleLine> SaleLines { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
    }
}