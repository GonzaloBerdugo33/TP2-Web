using System.ComponentModel.DataAnnotations;

namespace TPWeb.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "La direccion es obligatoria")]
    [StringLength(100, MinimumLength = 2)]
    public string Direccion { get; set; } = string.Empty;

    public List<Venta> Ventas { get; set; } = new();
}