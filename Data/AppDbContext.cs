using Microsoft.EntityFrameworkCore;
using TPWeb.Models;

namespace TPWeb.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<DetalleVenta> DetallesVenta => Set<DetalleVenta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>().Property(p => p.Precio).HasPrecision(18, 2);
        modelBuilder.Entity<DetalleVenta>().Property(d => d.PrecioUnitario).HasPrecision(18, 2);

        // No se puede borrar un proveedor con productos, ni un producto con ventas
        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Proveedor).WithMany(pr => pr.Productos)
            .HasForeignKey(p => p.ProveedorId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DetalleVenta>()
            .HasOne(d => d.Producto).WithMany(p => p.DetallesVenta)
            .HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.Restrict);

        // Si se borra una venta, se borran sus líneas de detalle
        modelBuilder.Entity<DetalleVenta>()
            .HasOne(d => d.Venta).WithMany(v => v.Detalles)
            .HasForeignKey(d => d.VentaId).OnDelete(DeleteBehavior.Cascade);
    }
}