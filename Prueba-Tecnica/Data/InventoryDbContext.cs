using Microsoft.EntityFrameworkCore;
using Prueba_Tecnica.Models;

namespace Prueba_Tecnica.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<ProductWarehouse> ProductWarehouses { get; set; }
    public DbSet<InventoryMovements> InventoryMovements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductWarehouse>(entity =>
        {
            entity.ToTable(t => t.HasCheckConstraint("CK_CurrentStock_NonNegative", "[CurrentStock] >= 0"));

            entity.HasIndex(pw => new { pw.ProductId, pw.WarehouseId }).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}