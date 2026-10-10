using Microsoft.EntityFrameworkCore;
using TPWeb.Data;
using TPWeb.Models;

namespace TPWeb.Services;

public class VentaService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public VentaService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<Venta>> Listar()
    {
        using var db = _factory.CreateDbContext();
        return await db.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .OrderByDescending(v => v.Fecha)
            .ToListAsync();
    }

    public async Task<Venta> Confirmar(Cliente cliente, string tipoEntrega, string metodoPago, IReadOnlyList<CarritoItem> items)
    {
        if (items.Count == 0)
            throw new InvalidOperationException("El carrito está vacío");

        using var db = _factory.CreateDbContext();

        // Leer los productos reales desde la base
        var idsProductos = items.Select(i => i.Producto.Id).ToList();
        var productos = await db.Productos
            .Where(p => idsProductos.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        var venta = new Venta
        {
            Cliente = cliente,
            TipoEntrega = tipoEntrega,
            MetodoPago = metodoPago,
            Estado = "Confirmada"
        };

        // Validar stock, descontar y armar los detalles
        foreach (var item in items)
        {
            if (!productos.TryGetValue(item.Producto.Id, out var producto))
                throw new InvalidOperationException($"El producto \"{item.Producto.Nombre}\" ya no está disponible");

            if (producto.Stock < item.Cantidad)
                throw new InvalidOperationException($"No hay stock suficiente de \"{producto.Nombre}\" (quedan {producto.Stock})");

            producto.Stock -= item.Cantidad;

            venta.Detalles.Add(new DetalleVenta
            {
                ProductoId = producto.Id,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.Precio
            });
        }

        // Guardar todo junto
        db.Ventas.Add(venta);
        await db.SaveChangesAsync();

        return venta;
    }
}