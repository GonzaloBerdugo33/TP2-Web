# TPWeb – Pizzería

Aplicación web de venta de comida rápida hecha con **Blazor Server (.NET 10)**, **Entity Framework Core** y **PostgreSQL**.
Trabajo Práctico de **Programación Web** – Gonzalo Berdugo.

- **Sitio publicado:** http://tpweb-pizzeria.runasp.net
- **Casos de prueba (capturas del sitio):**![Casos De Prueba](docs/Casos_de_Prueba_TPWeb.pdf)

> La clave de administrador es "Admin123".

## Qué hace

**Cliente** (sin registro): ve el menú con imágenes, arma un carrito, elige retiro en mostrador o delivery, paga en efectivo o Mercado Pago y confirma el pedido. Los productos sin stock se muestran deshabilitados.

**Administrador** (con clave): gestiona **productos** (con subida de imagen), **clientes** y consulta los **pedidos** con su detalle. Estas secciones solo aparecen en el menú después de ingresar la clave.

## Reglas de negocio

- Una venta tiene **un cliente** y **varios productos** (cabecera `Venta` + líneas `DetalleVenta`).
- El **stock se descuenta al confirmar** la venta, validando y guardando todo en una sola operación.
- Cada línea guarda el **precio del momento** de la venta.
- En **delivery** la dirección es obligatoria; en retiro no se pide.
- No se puede eliminar un cliente con ventas ni un producto ya vendido.

## Modelo de datos

- Aca tenemos el diagrama de ER con sus respectivas tablas.

![Diagrama ER](docs/ER_TP2_WEB.pdf)

## Estructura

```
Models/      Cliente, Producto, Venta, DetalleVenta
Data/        AppDbContext
Migrations/  Migraciones de EF Core
Services/    Cliente, Producto, Imagen, Admin, Carrito, Venta
Components/  Layout, Shared (AdminGuard) y Pages
wwwroot/     Estilos e imágenes subidas (uploads/)
docs/        Diagrama ER y casos de prueba
```

## Ejecutar localmente

Requisitos: .NET SDK 10 y PostgreSQL.

1. Crear `appsettings.Development.json` (no se sube al repo):
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=tpweb;Username=postgres;Password=TU_PASSWORD"
     },
     "Admin": { "Clave": "TU_CLAVE_DE_ADMIN" }
   }
   ```
2. Ejecutar `dotnet run`. La aplicación aplica las migraciones al arrancar.

## Publicación

Publicado en **MonsterASP.NET** con base de datos PostgreSQL. La conexión y la clave de administrador se configuran en el servidor.
