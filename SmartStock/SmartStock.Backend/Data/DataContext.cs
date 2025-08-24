using Microsoft.EntityFrameworkCore;
using SmartStock.Shared.Entites;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SmartStock.Backend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ProductCode único
        modelBuilder.Entity<Product>()
            .HasIndex(x => x.ProductCode)
            .IsUnique();

        // CategoryName único
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.CategoryName)
            .IsUnique();
    }
}