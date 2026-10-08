using System.ComponentModel.DataAnnotations.Schema;

namespace TPWeb.Models;

public class Venta
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string Cliente { get; set; } = string.Empty;

    public List<DetalleVenta> Detalles { get; set; } = new();

    [NotMapped]
    public decimal Total => Detalles.Sum(d => d.Subtotal);
}