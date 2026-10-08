
using Microsoft.EntityFrameworkCore;
using TPWeb.Models;

namespace TPWeb.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<DetalleVenta> DetallesVenta => Set<DetalleVenta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>()
            .Property(p => p.Precio)
            .HasPrecision(18, 2);

        modelBuilder.Entity<DetalleVenta>()
            .Property(d => d.PrecioUnitario)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Venta>()
            .Property(v => v.CostoEnvio)
            .HasPrecision(18, 2);

        // Cada venta debe tener un cliente.
        // No se puede borrar un cliente que tenga ventas.
        modelBuilder.Entity<Venta>()
            .HasOne(v => v.Cliente)
            .WithMany(c => c.Ventas)
            .HasForeignKey(v => v.ClienteId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // No se puede borrar un producto que figure en ventas.
        modelBuilder.Entity<DetalleVenta>()
            .HasOne(d => d.Producto)
            .WithMany(p => p.DetallesVenta)
            .HasForeignKey(d => d.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Cada detalle pertenece a una venta.
        // Si se elimina una venta, se eliminan sus detalles.
        modelBuilder.Entity<DetalleVenta>()
            .HasOne(d => d.Venta)
            .WithMany(v => v.Detalles)
            .HasForeignKey(d => d.VentaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}