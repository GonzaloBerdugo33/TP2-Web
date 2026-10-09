using Microsoft.EntityFrameworkCore;
using TPWeb.Data;
using TPWeb.Models;

namespace TPWeb.Services
{
    public class ProductoService
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public ProductoService(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<Producto>> Listar()
        {
            using var db = _factory.CreateDbContext();
            return await db.Productos.OrderBy(p => p.Nombre).ToListAsync();
        }

        public async Task<Producto?> ObtenerPorId(int id)
        {
            using var db = _factory.CreateDbContext();
            return await db.Productos.FindAsync(id);
        }

        public async Task Crear(Producto producto)
        {
            using var db = _factory.CreateDbContext();
            db.Productos.Add(producto);
            await db.SaveChangesAsync();
        }

        public async Task Actualizar(Producto producto)
        {
            using var db = _factory.CreateDbContext();
            db.Productos.Update(producto);
            await db.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            using var db = _factory.CreateDbContext();
            bool tieneDetallesVentas = await db.DetallesVenta.AnyAsync(d => d.ProductoId == id);
            if (tieneDetallesVentas)
            {
                throw new InvalidOperationException("No se puede eliminar un producto que figure en ventas");
            }
            else
            {
                var producto = await db.Productos.FindAsync(id);
                if (producto != null)
                {
                    db.Productos.Remove(producto);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
