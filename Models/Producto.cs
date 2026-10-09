
using System.ComponentModel.DataAnnotations;

namespace TPWeb.Models;

public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    [Range(0.01, 1000000, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }

    [Range(0, 1000, ErrorMessage = "El Stock no debe ser negativo")]
    public int Stock { get; set; }

    public string? ImagenUrl { get; set; }

    public List<DetalleVenta> DetallesVenta { get; set; } = new();
}