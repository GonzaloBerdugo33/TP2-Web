namespace TPWeb.Services;

public class AdminService
{
    private readonly IConfiguration _config;

    public AdminService(IConfiguration config)
    {
        _config = config;
    }

    public bool EsAdmin { get; private set; }

    public event Action? Cambio;

    public bool Ingresar(string clave)
    {
        if (!string.IsNullOrEmpty(clave) && clave == _config["Admin:Clave"])
        {
            EsAdmin = true;
            Cambio?.Invoke();
            return true;
        }
        return false;
    }

    public void Salir()
    {
        EsAdmin = false;
        Cambio?.Invoke();
    }
}