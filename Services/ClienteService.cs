using Microsoft.EntityFrameworkCore;
using TPWeb.Data;
using TPWeb.Models;

namespace TPWeb.Services
{
    public class ClienteService
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public ClienteService(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<Cliente>> Listar()
        {
            using var db = _factory.CreateDbContext();
            return await db.Clientes.OrderBy(c => c.Nombre).ToListAsync();
        }

        public async Task<Cliente?> ObtenerPorId(int id)
        {
            using var db = _factory.CreateDbContext();
            return await db.Clientes.FindAsync(id);
        }

        public async Task Crear(Cliente cliente)
        {
            using var db = _factory.CreateDbContext();
            db.Clientes.Add(cliente);
            await db.SaveChangesAsync();
        }

        public async Task Actualizar(Cliente cliente) 
        {
            using var db = _factory.CreateDbContext();
            db.Clientes.Update(cliente);
            await db.SaveChangesAsync();
        }

        public async Task Eliminar(int id) 
        {
            using var db = _factory.CreateDbContext();
            bool tieneVentas = await db.Ventas.AnyAsync(v => v.ClienteId == id);
            if (tieneVentas)
            {
                throw new Exception("No se puede eliminar cliente por tener ventas asociadas");
            }
            else
            {
                var cliente = await db.Clientes.FindAsync(id);
                if (cliente != null)
                {
                    db.Clientes.Remove(cliente);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
