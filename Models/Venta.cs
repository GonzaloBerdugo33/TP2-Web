using System.ComponentModel.DataAnnotations.Schema;

namespace TPWeb.Models;

public class Venta
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    // Cliente obligatorio
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    // Estado de la venta
    public string Estado { get; set; } = "Pendiente";

    // Mostrador o Delivery
    public string TipoEntrega { get; set; } = "Mostrador";

    public decimal CostoEnvio { get; set; } = 0;

    // Efectivo o Mercado Pago
    public string MetodoPago { get; set; } = "Efectivo";

    public List<DetalleVenta> Detalles { get; set; } = new();

    [NotMapped]
    public decimal Total => Detalles.Sum(d => d.Subtotal) + CostoEnvio;
}