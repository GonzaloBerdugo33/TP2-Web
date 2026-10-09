using Microsoft.AspNetCore.Components.Forms;

namespace TPWeb.Services;

public class ImagenService
{
    private static readonly string[] Extensiones = { ".jpg", ".jpeg", ".png", ".webp" };
    private const long MaxBytes = 2 * 1024 * 1024; // 2 MB

    private readonly IWebHostEnvironment _env;

    public ImagenService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> Guardar(IBrowserFile archivo)
    {
        var extension = Path.GetExtension(archivo.Name).ToLowerInvariant();
        if (!Extensiones.Contains(extension))
            throw new InvalidOperationException("Formato de imagen no permitido");

        if (archivo.Size > MaxBytes)
            throw new InvalidOperationException("La imagen no puede pesar más de 2 MB");

        var carpeta = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(carpeta);

        var nombre = $"{Guid.NewGuid()}{extension}";
        var ruta = Path.Combine(carpeta, nombre);

        await using var origen = archivo.OpenReadStream(MaxBytes);
        await using var destino = new FileStream(ruta, FileMode.Create);
        await origen.CopyToAsync(destino);

        return $"/uploads/{nombre}";
    }
}